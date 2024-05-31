import StateStore from "./StateStore";
import { Status } from "../../enums";
import { ServerError } from "../../errors";

/**  класс состояния стора */
class ErrorStateStore extends StateStore {
  constructor(error?: unknown) {
    super();
    this.status = Status.Error;
    this.error =
      (error as ServerError)?.title || "Произошла непредвиденная ошибка";
    this.errorCode = (error as ServerError)?.status;
  }
}
export default ErrorStateStore;
