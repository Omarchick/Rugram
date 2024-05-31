import { createContext } from "react";
import UserStore from "../stores/userStore";
import ModalStore from "../stores/ModalStore";
import UploadStore from "../stores/UploadStore";
import FeedStore from "../stores/FeedStore";

export const storeContext = createContext({
  userStore: new UserStore(),
  modalStore: new ModalStore(),
  uploadStore: new UploadStore(),
  feedStore: new FeedStore(),
});
