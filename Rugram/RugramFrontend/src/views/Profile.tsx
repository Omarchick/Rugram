/* eslint-disable @typescript-eslint/no-explicit-any */
import { FC, useEffect, useMemo, useState } from "react";
import ProfileCard from "../components/ProfileCard";
import UseStores from "../hooks/useStores";
import { useNavigate, useParams } from "react-router";
import { observer } from "mobx-react";
import ModalSearchWindow from "../components/ModalWindow";
import { CropperContainer } from "./Add";
import Cropper from "react-easy-crop";
import Button from "../components/ui/Button";
import getCroppedImg from "../tools/cropImage";
import CardGrid from "../components/CardGrid";
import styled from "styled-components";

const ProfilePage = styled.div`
  display: flex;
  gap: 24px;
`

const cropperStyle = {
  containerStyle: { position: 'relative', height: '30vw', width: '30vw', borderRadius: '16px', marginBottom: '24px' } as React.CSSProperties,
}

const Profile: FC = () => {
  const { userStore, uploadStore } = UseStores();

  const navigate = useNavigate();
  const { id } = useParams()
  const [crop, setCrop] = useState({ x: 0, y: 0 })
  const [rotation, setRotation] = useState(0)
  const [zoom, setZoom] = useState(1)
  const [croppedImage, setCroppedImage] = useState('')
  const [croppedAreaPixels, setCroppedAreaPixels] = useState<
  {x: number, y: number, width: number, height: number}
  >({x: 0, y: 0, width: 50, height: 50})

  useEffect(() => {
    if (['/profile/', '/profile'].includes(window.location.pathname)) {
      navigate(`/profile/${userStore.user.id}`, { replace: true });
    };
    return () => {
      userStore.clearUser()
      userStore.user.posts = [];
    }
  }, [])

  useEffect(() => {
    if (id) {
      userStore.getProfile(id);
    }
  }, [id])

  const onCropComplete = (croppedArea: any, areaPixels: any) => {
    setCroppedAreaPixels(areaPixels)
  }

  const changeImage = async (image: string) => {
    if (image && id) {
      await uploadStore.changeImage(image);
      await userStore.getProfile(id)
    }
  }

  const showCroppedImage = async () => {
    try {
      const croppedImageFile = await getCroppedImg(
        {
          imageSrc: uploadStore.currentImage,
          pixelCrop: {
            x: croppedAreaPixels.x,
            y: croppedAreaPixels.y,
            width: croppedAreaPixels.width,
            height: croppedAreaPixels.height
          },
        }
      );
      setCroppedImage(croppedImageFile)
      return croppedImageFile;
    } catch (e) {
      console.error(e)
    }
  }

  const CropperFunc = useMemo(() => {
    return (
      <CropperContainer>
        <Cropper
          aspect={1}
          crop={crop}
          cropShape="round"
          image={uploadStore.currentImage}
          onCropChange={setCrop}
          onCropComplete={onCropComplete}
          onRotationChange={setRotation}
          onZoomChange={setZoom}
          rotation={rotation}
          style={cropperStyle}
          zoom={zoom}
          zoomSpeed={0.1}
        />
        <Button
          onClick={async () => {
            await changeImage(await showCroppedImage() ?? '');
          }}
          text='Готово'
        />
      </CropperContainer>
    )
  }, [uploadStore.currentImage, crop, rotation, zoom, setCrop, onCropComplete, setRotation, setZoom, croppedImage])

  const MemoCardGrid = useMemo(() => {
    return (
      <CardGrid />
    )
  }, [window.location, userStore.user.username])

  return (
    <ProfilePage>
      <ProfileCard
        isSameUser={userStore.user.id === id}
      />
      {MemoCardGrid}
      {
        uploadStore.currentImage && (
          <ModalSearchWindow
            onClose={() => uploadStore.currentImage = ''}
            title='Маштабирование'
          >
            {CropperFunc}
          </ModalSearchWindow>
        )
      }
    </ProfilePage>
  );
};

export default observer(Profile);
