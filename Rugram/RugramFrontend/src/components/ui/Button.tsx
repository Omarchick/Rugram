import { ChangeEvent, FC, useRef, useState } from "react";
import styled from "styled-components";
import UseStores from "../../hooks/useStores";
import { icons } from "../../enums";
import ErrorModal from "../ErrorModal";

interface ButtonProps {
  text: string;
  onClick: () => void;
}

const StyledButton = styled.button`
  border-radius: 8px;
  padding: 16px;
  font-size: 16px;
  font-weight: 600;
  width: -webkit-fill-available;

  cursor: pointer;

  background: linear-gradient(
    135deg,
    rgba(255, 255, 255, 0.1),
    rgba(255, 255, 255, 0)
  );
  backdrop-filter: blur(10px);
  -webkit-backdrop-filter: blur(10px);
  border: 2px solid black;
  box-shadow: 0 8px 32px 0 rgba(0, 0, 0, 0.37);
`;

const StyledUploadButtonContainer = styled.button`
  border-radius: 12px;
  padding: 16px;
  font-size: 16px;
  font-weight: 600;
  width: 20vw;
  height: 20vw;

  background: linear-gradient(
    135deg,
    rgba(255, 255, 255, 0.1),
    rgba(255, 255, 255, 0)
  );
  backdrop-filter: blur(10px);
  -webkit-backdrop-filter: blur(10px);
  border: 2px solid black;
  box-shadow: 0 8px 32px 0 rgba(0, 0, 0, 0.37);
`;

const PlusButtonCon = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 16px;
  background: none;
  border: none;
  width: 100%;
  height: 100%;
  cursor: pointer;
`;

export const Button: FC<ButtonProps> = ({ onClick, text }) => {
  return <StyledButton onClick={onClick}>{text}</StyledButton>;
};

export const UploadButton : FC = () => {
  const { uploadStore } = UseStores()
  const [uploadError, setUploadError] = useState('')
  const uploadRef = useRef<HTMLInputElement>(null)

  const handleUpload = (e: ChangeEvent<HTMLInputElement>) => {

    if (e.target.files === null) {
      return
    }
    const file = e.target.files[0]

    if (file) {
      if (!['image/png', 'image/jpeg', 'image/jpg'].includes(file.type)) {
        setUploadError('Для загрузки доступны только файлы формата .png/.jpeg/.jpg')
        return;
      }

      const fileReader = new FileReader()
      fileReader.onload = (event) => {
        const contents = event?.target?.result
        uploadStore.currentImage = contents as string;
      }

      e.target.value = ''
      fileReader.readAsDataURL(file)
    } else {
      setUploadError('Ошибка загрузки файла. Попробуйте ещё раз.')
    }
  }

  return (
    <StyledUploadButtonContainer>
      <PlusButtonCon onClick={() => {
        setUploadError('')
        uploadRef.current?.click()
      }}
      >
        <img
          alt="plus"
          src={icons.plus}
        />
        Загрузить
      </PlusButtonCon>

      <input
        ref={uploadRef}
        accept='image/*'
        multiple={false}
        onChange={handleUpload}
        style={{ display: 'none' }}
        type="file"
      />

      {uploadError ? <ErrorModal error={uploadError} /> : null}
    </StyledUploadButtonContainer>
  )
}

export default Button;
