export const dataURLtoFile =  (dataurl: string) => {
  const splitDataURI = dataurl.split(',')
  const mimeString = splitDataURI[0].match(/:(.*?);/)![1];
  const data = splitDataURI[1];

  const dataString = atob(data);
  let n = dataString.length;
  const dataArr = new Uint8Array(n);

  while (n--) {
    dataArr[n] = dataString.charCodeAt(n);
  }
  return new File([dataArr], '@image.jpeg', { type: mimeString });
}