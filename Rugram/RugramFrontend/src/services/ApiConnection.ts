import axios from "axios";

/** Базовый URL */
export const appApiUrl = "https://localhost:3001";

/** Базовые настройки axios */
export const ApiConnection = axios.create({
  baseURL: `${appApiUrl ?? ""}`,
  headers: {
    "Cache-Control": "no-cache",
    // "Access-Control-Allow-Origin": "*",
    "Content-Type": "application/json",
  },
  maxContentLength: 512 * 1024 * 1024,
});

const unblobError = async (error: unknown) =>
  error instanceof Blob ? JSON.parse(await error.text()) : error;

/** Порядок обработки ошибок ответа сервера */
ApiConnection.interceptors.response.use(
  (response) => {
    if (response.status === 401 || response.status === 400) {
      throw response;
    }
    return response;
  },
  async (error) => {
    const errors = error?.response?.data;
    throw await unblobError(errors);
  },
);

/** Токен авторизации */
export const getToken = () => localStorage.getItem("rugramToken") || "";

/** Очистка токена */
export const removeToken = () => localStorage.removeItem("rugramToken");

/** Вставка токена в хедер запроса */
ApiConnection.interceptors.request.use((config) => {
  const token = getToken();
  // eslint-disable-next-line no-param-reassign
  config.headers!.Authorization = `Bearer ${token}`;
  return config;
});
