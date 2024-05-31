// import axios from "axios";
import { dataURLtoFile } from "../tools/imgConverter";
import { getToken } from "./ApiConnection";

class UploadService {
  static async CreatePost(images: string[], description: string) {
    const formData = new FormData();
    images.map((photo, index) => {
      formData.append("photos", dataURLtoFile(photo), `photo${index}.jpeg`);
    })
    formData.append("description", description);
    const response = await fetch("https://localhost:3001/post",
      {
        method: "POST",
        headers: {
          // 'Content-Type': 'multipart/form-data',
          'Authorization': `Bearer ${getToken()}`,
          'accept': '*/*'
        },
        body: formData
      },);
    return response;
  }

  static async ChangeImage(image: string) {
    const formData = new FormData();
    formData.append("profilePhoto", dataURLtoFile(image), `photo.jpeg`);
    const response = await fetch("https://localhost:3001/profile/editProfilePhoto",
      {
        method: "POST",
        headers: {
          // 'Content-Type': 'multipart/form-data',
          'Authorization': `Bearer ${getToken()}`,
          'accept': '*/*'
        },
        body: formData
      },);
    return response;
  }
}

export default UploadService;
