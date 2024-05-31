import { makeAutoObservable } from "mobx";
import UploadService from "../services/UploadServices";
import StateStore from "./StateStores/StateStore";
import FetchingStateStore from "./StateStores/FetchingStateStore";
import ErrorStateStore from "./StateStores/ErrorStateStore";
import SuccessStateStore from "./StateStores/SuccessStateStore";

class UploadStore {
  _images: string[];

  _currentImage?: string;

  state?: StateStore;

  constructor() {
    this._images = [];
    makeAutoObservable(this)
  }

  public get images() {
    return this._images;
  }

  public set images(image: string[]) {
    this._images = [...(new Set([...this._images, ...image]))]
  }

  public get currentImage() {
    return this._currentImage ?? '';
  }

  public set currentImage(image: string) {
    this._currentImage = image;
  }

  public async createPost(description: string) {
    this.state = new FetchingStateStore();
    try {
      const response = await UploadService.CreatePost(this.images, description);
      if (response.status === 204) {
        this._images = [];
        this._currentImage = undefined;
        this.state = new SuccessStateStore();
      }
    } catch (error) {
      this.state = new ErrorStateStore();
      console.log(error);
    }
  }

  public async changeImage(image: string) {
    this.state = new FetchingStateStore();
    try {
      const response = await UploadService.ChangeImage(image);
      if (response.status === 204) {
        this._images = [];
        this._currentImage = undefined;
        this.state = new SuccessStateStore();
      }
    } catch (error) {
      this.state = new ErrorStateStore();
      console.log(error);
    }
  }

  public clearStore() {
    this.images = [];
    this._currentImage = undefined;
  }
}

export default(UploadStore);