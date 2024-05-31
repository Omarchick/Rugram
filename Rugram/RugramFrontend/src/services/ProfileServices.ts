import { Posts, ProfilePost, ProfileRequest, SearchProfile, SinglePost, SubInfoType } from "../types/commonTypes";
import { ApiConnection } from "./ApiConnection";

class ProfileServices {
  static async Follow(id: string) {
    const response = await ApiConnection.put(`profile/subscribe/${id}`);
    return response;
  }

  static async UnFollow(id: string) {
    const response = await ApiConnection.put(`profile/unsubscribe/${id}`);
    return response;
  }

  static async GetProfile(id: string) {
    const name = await ApiConnection.get<{profileName: string}>(`profile/profileName/${id}`);
    const icon = await ApiConnection.get<{profilePhoto: string}>(`profile/profilePhoto/${id}`);
    let subInfo : SubInfoType | undefined;
    try {
      subInfo = (await ApiConnection.get<SubInfoType>(`profile/subInfo/${id}`)).data;
    } catch(e) {
      console.log(e);
    }
    const followers = await ApiConnection.get<{
      subscribersCount: number,
      subscriptionsCount: number
    }>(`profile/profileIndicators/${id}`);
    return { ...name.data, icon, ...followers.data, subInfo } as unknown as ProfileRequest;
  }

  static async GetPostInfo(id: string) {
    const response = await ApiConnection.get<SinglePost>(`post/${id}`);
    return response.data;
  }

  static async GetProfileImg(id: string) {
    const response = await ApiConnection.get<{profilePhoto: string}>(`profile/profilePhoto/${id}`);
    return response.data;
  }

  static async GetPosts(id: string, pageNumber: number, pageSize: number) {
    const response = await ApiConnection.get<Posts>(`post/${id}&${pageNumber}&${pageSize}`);
    return response.data;
  }

  static async GetPostImage(id: string, photoId: string) {
    const response = await ApiConnection.get<{photo: string}>(`post/photo/${photoId}&${id}`);
    return response.data.photo;
  }

  static async Search(search: string) {
    const response = await ApiConnection.get<SearchProfile>(`profile/recommendations/8&0/${search}`);
    return response.data;
  }

  static async GetFeed(pageNumber: number, pageSize: number) {
    const response = await ApiConnection.get<{feedPostDto : ProfilePost[]}>(`profile/feed/${pageSize}&${pageNumber}`);
    return response.data
  }
}

export default ProfileServices;
