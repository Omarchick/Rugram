export const regexp = {
  email:
    /^([a-zA-Z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_-]+(\.[a-z0-9_-]+)*\.[a-z]{2,6}$/i,
  password: /(?=^.{6,}$)(?=.*[0-9])(?=.*[A-Z])(?=.*[a-z])/g,
  login: /^[a-zA-Z0-9_]*$/,
};
