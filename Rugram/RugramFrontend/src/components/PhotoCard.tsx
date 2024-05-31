import { FC, useCallback } from "react";
import styled from "styled-components";
import { GlassDiv } from "../styles";
import Flickity from 'react-flickity-component'

import "../styles/flickity.css";
import Separator from "./ui/Separator";
import { observer } from "mobx-react";
import { useNavigate } from "react-router-dom";

const Slider = styled(Flickity)`
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;
  gap: 4px;
`;

const Description = styled.div`
  text-overflow: ellipsis;
  white-space: nowrap;
  overflow: hidden;
  width: -webkit-fill-available;
  font-size: 14px;
  font-weight: 400;
  padding: 0 12px;
`;

const Photo = styled.div`
  width: 100%;
  height: 100%;
`

const PostContainer = styled(GlassDiv)<{withSlider: boolean}>`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  border-radius: 12px;
  padding: 8px;
  gap: 12px;

  cursor: pointer;

  max-width: 20vw;
  height: min-content;

  img {
    width: 100%;
    max-width: 19vw;
    height: 100%;
    max-height: 19vw;
    border-radius: 12px;
  }
`

const PhotoCard: FC<{
  src: string[] | string,
  description?: string,
  link?: string,
}> = ({src, description, link}) => {
  const withSlider = typeof src !== "string";
  const navigate = useNavigate();

  const MemoSlider = useCallback((images : string[]) => {
    return (
      <Photo
        // eslint-disable-next-line @typescript-eslint/ban-ts-comment
        // @ts-ignore
        onClick={(e: Event) => e.stopPropagation()}
      >
        <Slider
          static
        >
          {images ? images.map((image) => (
            <img
              key={image}
              src={`data:image/png;base64, ${image}`}
            />
          )) : null}
        </Slider>
      </Photo>
    )
  }, [src])

  return (
    <PostContainer
      onClick={() => {
        if (link) {
          navigate(`/profile/post/${link}`)
        }
      }}
      withSlider={withSlider}
    >
      {!withSlider
        ? (
          <img src={`${src}`}/>
        ) : (
          <>
            {MemoSlider(src)}
            <Separator />
            <Description>{description}</Description>
          </>
        )}
    </PostContainer>
  );
};

export default observer(PhotoCard);
