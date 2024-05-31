import { computed, observable } from "mobx";
import { Status } from "../../enums";

class StateBaseStore {
  @observable status: Status;

  @observable error?: string;

  @observable errorCode?: number;

  constructor() {
    this.status = Status.Initial;
  }

  @computed get isLoading(): boolean {
    return this.status === Status.Fetching;
  }

  @computed get isSuccess(): boolean {
    return this.status === Status.Success;
  }

  @computed get isError(): boolean {
    return this.status === Status.Error;
  }

  @computed get isInitial(): boolean {
    return this.status === Status.Initial;
  }

  @computed get isAutoSaveCanceled() {
    return this.isError && this.errorCode === 409;
  }
}
export default StateBaseStore;
