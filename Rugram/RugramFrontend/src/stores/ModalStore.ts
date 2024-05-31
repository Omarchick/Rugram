import { makeAutoObservable } from "mobx";
import { ReactNode } from "react";

class ModalStore {
  isOpen: boolean;

  title?: string;

  content?: ReactNode;

  onClose?: () => void

  constructor() {
    makeAutoObservable(this);
    this.isOpen = false;
  }

  public toggleModal() {
    this.isOpen = !this.isOpen;
  }

  public closeModal() {
    this.isOpen = false;
  }

  public onCloseModal() {
    this.isOpen = false;
    this?.onClose?.();
  }

  public clearStore() {
    this.isOpen = false;
    this.title = undefined;
    this.content = undefined;
  }
}

export default ModalStore;
