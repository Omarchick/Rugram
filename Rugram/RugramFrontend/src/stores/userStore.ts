import { makeAutoObservable, runInAction } from "mobx";
import Authorization from "../services/AuthServices";
import StateStore from "./StateStores/StateStore";
import FetchingStateStore from "./StateStores/FetchingStateStore";
import ErrorStateStore from "./StateStores/ErrorStateStore";
import SuccessStateStore from "./StateStores/SuccessStateStore";
import { decodeToken } from "../tools/decodeToken";
import ProfileServices from "../services/ProfileServices";
import { SearchProfile, SinglePost, User } from "../types/commonTypes";

class UserStore {

  user: User;

  state?: StateStore;

  token?: string;

  refreshToken?: string;

  searchProfiles?: SearchProfile;

  singlePost?: SinglePost;

  subInfo?: {
    otherProfileSubscribedToThisProfile: boolean,
    thisProfileSubscribedToOtherProfile: boolean
  }

  constructor() {
    makeAutoObservable(this);
    this.token = localStorage.getItem("rugramToken") ?? undefined;
    this.refreshToken = localStorage.getItem("refreshToken") ?? undefined;
    this.user = {
      id: undefined,
      username: undefined,
      img: undefined,
      followersCount: undefined,
      followingCount: undefined,
      description: undefined,
    };
    this.user.id = this.token ? decodeToken(this.token) : undefined;
  }

  public async SendEmail(email: string) {
    this.state = new FetchingStateStore();
    try {
      await Authorization.SendEmail(email);
      this.state = new SuccessStateStore();
      return true;
    } catch (error) {
      runInAction(() => {
        this.state = new ErrorStateStore(error);
      });
    }
  }

  public async CheckProfileName(profileName: string) {
    this.state = new FetchingStateStore();
    try {
      await Authorization.checkNameAvailability(profileName);
      this.state = new SuccessStateStore();
      return true;
    } catch (error) {
      runInAction(() => {
        this.state = new ErrorStateStore(error);
      });
    }
  }

  public async CheckMail(mail: string) {
    this.state = new FetchingStateStore();
    try {
      this.state = new SuccessStateStore();
      return await Authorization.checkMailAvailability(mail);
    } catch (error) {
      runInAction(() => {
        this.state = new ErrorStateStore(error);
      });
    }
  }

  public async Registration(
    token: string,
    email: string,
    password: string,
    profileName: string,
  ) {
    this.state = new FetchingStateStore();
    try {
      const response = await Authorization.Registration(
        token,
        email,
        password,
        profileName,
      );
      if (response) {
        localStorage.setItem("rugramToken", response.jwtToken);
        localStorage.setItem("refreshToken", response.refreshToken);
        this.token = response.jwtToken;
        this.refreshToken = response.refreshToken;
        this.state = new SuccessStateStore();
      }
    } catch (error) {
      runInAction(() => {
        this.state = new ErrorStateStore(error);
      });
    }
  }

  public async Login(email: string, password: string) {
    this.state = new FetchingStateStore();
    try {
      const response = await Authorization.Login(email, password);
      if (response) {
        localStorage.setItem("rugramToken", response.jwtToken);
        localStorage.setItem("refreshToken", response.refreshToken);
        this.token = response.jwtToken;
        this.refreshToken = response.refreshToken;
        this.state = new SuccessStateStore();
      }
    } catch (error) {
      runInAction(() => {
        this.state = new ErrorStateStore(error);
      });
    }
  }

  public LogOut() {
    this.token = undefined;
    this.refreshToken = undefined;
    localStorage.removeItem("rugramToken");
    localStorage.removeItem("refreshToken");
  }

  public async updateJwt() {
    this.state = new FetchingStateStore();
    try {
      const response = await Authorization.updateToken(this.token!, this.refreshToken!);
      if (response) {
        localStorage.setItem("rugramToken", response.jwtToken);
        this.token = response.jwtToken;
        this.state = new SuccessStateStore();
      }
    } catch (error) {
      runInAction(() => {
        this.state = new ErrorStateStore(error);
      });
    }
  }

  public async follow(id: string) {
    this.state = new FetchingStateStore();
    try {
      await ProfileServices.Follow(id);
      this.state = new SuccessStateStore();
    } catch (error) {
      runInAction(() => {
        this.state = new ErrorStateStore(error);
      });
    }
  }

  public async unFollow(id: string) {
    this.state = new FetchingStateStore();
    try {
      await ProfileServices.UnFollow(id);
      this.state = new SuccessStateStore();
    } catch (error) {
      runInAction(() => {
        this.state = new ErrorStateStore(error);
      });
    }
  }

  public async getProfile(id: string) {
    this.state = new FetchingStateStore();
    try {
      const response = await ProfileServices.GetProfile(id);
      runInAction(() => {
        this.user.username = response.profileName
        this.user.img = response.icon.status === 200
          ? `data:image/png;base64, ${response.icon.data.profilePhoto}`
          : undefined;
        this.user.followersCount = response.subscribersCount;
        this.user.followingCount = response.subscriptionsCount;
        this.subInfo = response.subInfo;
      })

      this.state = new SuccessStateStore();
    } catch (error) {
      runInAction(() => {
        this.state = new ErrorStateStore(error);
      });
    }
  }

  public async getPostProfile(id:string) {
    this.state = new FetchingStateStore();
    try {
      const response = await ProfileServices.GetProfile(id);
      this.state = new SuccessStateStore();
      return response;
    } catch (error) {
      runInAction(() => {
        this.state = new ErrorStateStore(error);
      });
    }
  }

  public async getPosts(id:string, pageNumber: number) {
    this.state = new FetchingStateStore();
    try {
      if (this.user.id) {
        const response = await ProfileServices.GetPosts(id, pageNumber, 10);
        const photoPromises = response.posts.map(async (post) => {
          post.photoUrls = await Promise.all(post.photoIds.map(async (photo) => {
            return await ProfileServices.GetPostImage(id!, photo)
          }));
          return post;
        });
        await Promise.all(photoPromises);
        this.user.postsCount = response.totalPostsCount;
        this.user.posts = [...(this.user.posts ?? []), ...response.posts]
      }

      this.state = new SuccessStateStore();
    } catch (error) {
      this.state = new ErrorStateStore(error);
    }
  }

  public async getSinglePost(id:string) {
    this.state = new FetchingStateStore();
    try {
      const response = await ProfileServices.GetPostInfo(id);
      this.state = new SuccessStateStore();
      this.singlePost = response;
    } catch (error) {
      runInAction(() => {
        this.state = new ErrorStateStore(error);
      });
    }
  }

  public async search(search: string) {
    this.state = new FetchingStateStore();
    try {
      const response = await ProfileServices.Search(search);
      this.searchProfiles = response;

      const photoPromises = this.searchProfiles.profiles.map(async (profile) => {
        profile.profileImg = (await ProfileServices.GetProfileImg(profile.id)).profilePhoto;
      })
      await Promise.all(photoPromises);
      this.state = new SuccessStateStore();
    } catch (error) {
      this.state = new ErrorStateStore(error);
    }
  }

  // public async checkSubscriptions(profileId: string) {
  //   this.state = new FetchingStateStore();
  //   try {
  //     const response = await ProfileServices.CheckSubscriptions(profileId);
  //     this.state = new SuccessStateStore();
  //     return response;
  //   } catch (error) {
  //     this.state = new ErrorStateStore(error);
  //   }
  // }

  public async clearUser() {
    this.user.username = undefined;
    this.user.followersCount = undefined;
    this.user.followingCount = undefined;
    this.user.description = undefined;
    this.user.img = undefined;
    this.user.posts = undefined;
    this.subInfo = undefined;
  }
}

export default UserStore;
