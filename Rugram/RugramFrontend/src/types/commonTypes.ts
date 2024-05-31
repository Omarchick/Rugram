import { AxiosResponse } from "axios";

export interface JwtResponseType {
  jwtToken: "string";
  refreshToken: "string";
}

export type WithValidation = {
  validate(): string | undefined;
};

export type ProfileRequest = {
  profileName: string,
  icon: AxiosResponse<{
    profilePhoto: string;
  }>,
  subscribersCount: number,
  subscriptionsCount: number
  subInfo: SubInfoType
}

export type User = {
  id?: string,
  username?: string,
  img?: string,
  description?: string,
  followersCount?: number,
  followingCount?: number,
  posts?: Post[],
  postsCount?: number,
}

export type Posts = {
  posts: Post[],
  totalPostsCount: number,
}

export type Post = {
  postId: string,
  description: string,
  dateOfCreation: string,
  photoIds: string[],
  photoUrls?: string[],
}

export type SinglePost = {
  profileId: string,
  description: string,
  dateOfCreation: string,
  photos: string[],
}


export type ProfilePost = {
  profileId: string,
  postId: string,
  description: string,
  dateOfCreation: string,
  photoIds: string[],
  photoUrls?: string[],
}

export type SearchProfile = {
  profiles: Array<{
    id: string,
    profileName: string,
    profileImg?: string,
  }>
}

export type SubInfoType = {
  otherProfileSubscribedToThisProfile: boolean,
  thisProfileSubscribedToOtherProfile: boolean
}

export type CommentType = {
  profileId: string,
  profileName: string,
  profileIcon: string,
  commentId: string,
  commentText: string,
}