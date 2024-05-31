/* eslint-disable @typescript-eslint/no-explicit-any */
import { createRef, FC, useCallback, useEffect, useMemo, useState } from 'react';
import Cropper from 'react-easy-crop';
import getCroppedImg from '../tools/cropImage';
import useStores from '../hooks/useStores';
import ModalSearchWindow from '../components/ModalWindow';
import { observer } from 'mobx-react';
import styled from 'styled-components';
import Button, { UploadButton } from '../components/ui/Button';
import PhotoCard from '../components/PhotoCard';
import { runInAction } from 'mobx';
import TextArea from '../components/ui/TextArea';
import { WithValidation } from '../types/commonTypes';
import { useNavigate } from 'react-router-dom';

export const CropperContainer = styled.div`
  width: 30vw;
  height: 35vw;
`

const PostGrid = styled.div`
  width: 100%;
  display: grid;
  grid-template-columns: repeat(4, 20vw);
  gap: 3vw;
  flex-wrap: wrap;
`

const PageContainer = styled.div`
  display: flex;
  gap: 36px;
  flex-direction: column;
  justify-content: space-between;
  min-height: 85vh;
`

const DescriptionArea = styled.div`
  display: flex;
  flex-direction: column;
  gap: 24px;
  width: 100%;
`

const cropperStyle = {
  containerStyle: { position: 'relative', height: '30vw', width: '30vw', borderRadius: '16px', marginBottom: '24px' } as React.CSSProperties,
}

const ImageCropper: FC = () => {

  const { uploadStore } = useStores();

  const [crop, setCrop] = useState({ x: 0, y: 0 })
  const [rotation, setRotation] = useState(0)
  const [zoom, setZoom] = useState(1)
  const [croppedImage, setCroppedImage] = useState('')
  const [croppedAreaPixels, setCroppedAreaPixels] = useState<
  {x: number, y: number, width: number, height: number}
  >({x: 0, y: 0, width: 50, height: 50})
  const [description, setDescription] = useState('');
  const [descriptionError, setDescriptionError] = useState('');
  const descriptionRef = createRef<WithValidation>();

  const navigate = useNavigate();

  useEffect(() => {
    runInAction(() => {
      if (croppedImage.length) {
        uploadStore.images = [...uploadStore.images, croppedImage]
        uploadStore.currentImage = '';
      }
    })
  }, [croppedImage])

  const onCropComplete = (croppedArea: any, areaPixels: any) => {
    setCroppedAreaPixels(areaPixels)
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
      )
      setCroppedImage(croppedImageFile)
    } catch (e) {
      console.error(e)
    }
  }

  const checkErrors = () => {
    const descriptionErrors = descriptionRef.current?.validate();
    if (descriptionErrors) {
      setDescriptionError(descriptionError);
    }
    if (uploadStore.images.length === 0) {
      setDescriptionError('Необходимо загрузить изображение');
    }
    return !!descriptionErrors?.length || uploadStore.images.length === 0;
  };

  const createPost = useCallback(async () => {
    if (!checkErrors()) {
      await uploadStore.createPost(description);
      setDescription('');
      if (uploadStore.state?.isSuccess) {
        navigate('/profile')
      }
    }
  }, [uploadStore._images, description, descriptionError])

  const CropperFunc = useMemo(() => {
    return (
      <CropperContainer>
        <Cropper
          aspect={1 / 1}
          crop={crop}
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
            await showCroppedImage();
          }}
          text='Готово'
        />
      </CropperContainer>
    )
  }, [uploadStore.currentImage, crop, rotation, zoom, setCrop, onCropComplete, setRotation, setZoom, croppedImage])

  const Cards = useCallback(() => {
    return (
      uploadStore._images.map((image: string) => {
        return (
          <PhotoCard
            key={image.substring(10, 35)}
            src={image}
          />
        )
      })
    )
  }, [uploadStore.images, croppedImage])

  return (
    <PageContainer>
      <PostGrid>
        {uploadStore._images.length < 10 && (
          <UploadButton />
        )}
        {uploadStore.images &&
          Cards()
        }
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
      </PostGrid>
      <DescriptionArea>
        <TextArea
          ref={descriptionRef}
          errorMessage={descriptionError}
          maxLength={3000}
          minLength={1}
          onChange={(value) => {
            setDescription(value)
            setDescriptionError('');
          }}
          title={'Описание'}
          type='text'
          value={description}
        />
        <Button
          onClick={() => createPost()}
          text={'Создать'}
        />
      </DescriptionArea>
    </PageContainer>
  );
};

export default observer(ImageCropper);
