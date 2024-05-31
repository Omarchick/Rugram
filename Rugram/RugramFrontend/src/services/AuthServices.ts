import { JwtResponseType } from "../types/commonTypes";
import { ApiConnection } from "./ApiConnection";

class Authorization {
  static async Login(email: string, password: string) {
    const response = await ApiConnection.post<JwtResponseType>("/auth/login", {
      email: email,
      password: password,
    });
    return response.data;
  }

  static async SendEmail(email: string) {
    const response = await ApiConnection.post("/auth/confirm-email", {
      email: email,
    });
    return response.data;
  }

  static async Registration(
    token: string,
    email: string,
    password: string,
    profileName: string,
  ) {
    const response = await ApiConnection.post<JwtResponseType>(
      "/auth/registr-user",
      {
        mailConfirmationToken: token,
        email: email,
        password: password,
        profileName: profileName,
      },
    );
    return response.data;
  }

  static async checkNameAvailability(profileName: string) {
    const response = await ApiConnection.get(
      `/profile/checkProfileNameAvailability/${profileName}`,
    );
    return response.status === 204;
  }

  static async checkMailAvailability(mail: string) {
    const response = await ApiConnection.get(
      `/auth/checkEmailAvailability/${mail}`,
    );
    return response.status === 204;
  }

  static async updateToken(token: string, refreshToken: string) {
    const response = await ApiConnection.put<{jwtToken: string}>('/auth/update-jwt', {
      oldJwtToken: token,
      refreshToken: refreshToken,
    })
    return response.data;
  }
}

export default Authorization;
