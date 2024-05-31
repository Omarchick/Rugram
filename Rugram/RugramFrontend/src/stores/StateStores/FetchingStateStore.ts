import StateStore from "./StateStore";
import { Status } from "../../enums";
/**  класс состояния стора */
class FetchingStateStore extends StateStore {
  constructor() {
    super();
    this.status = Status.Fetching;
  }
}
export default FetchingStateStore;
