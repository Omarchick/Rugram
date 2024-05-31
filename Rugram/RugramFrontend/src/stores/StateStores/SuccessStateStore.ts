import StateStore from "./StateStore";
import { Status } from "../../enums";
/**  класс состояния стора */
class SuccessStateStore extends StateStore {
  constructor() {
    super();
    this.status = Status.Success;
  }
}
export default SuccessStateStore;
