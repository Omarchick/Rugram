import { FC } from "react";
import styled from "styled-components";
import video from "../assets/background/background_video.mp4";

const Video = styled.video`
  position: fixed;
  right: 0;
  bottom: 0;
  min-width: 100%;
  min-height: 100%;
  width: auto;
  height: auto;
  z-index: -9999;
`;

const BackgroundVideo: FC = () => {
  return (
    <Video
      autoPlay
      loop
      muted
    >
      <source
        src={video}
        type="video/mp4"
      />
    </Video>
  );
};

export default BackgroundVideo;
