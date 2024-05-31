/** Интерфейс для объекта ошибки */
export interface ServerError extends Error {
  /** Тип ошибки */
  type?: string;

  /** Заголовок ошибки */
  title?: string;

  /** Номер статуса ошибки */
  status?: number;

  /** Детали ошибки */
  detail?: string;

  /** Пример ошибки */
  instance?: string;

  /** id ошибки */
  errorId?: string;
}

export class MissingIdError extends Error {
  title: string;

  constructor(path: string, ...params: (string | undefined)[]) {
    super(...params);
    if (Error.captureStackTrace) {
      Error.captureStackTrace(this, MissingIdError);
    }
    this.title = `при отправке запроса ${path} id оказалось пустым`;
  }
}
