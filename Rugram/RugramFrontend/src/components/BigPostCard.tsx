import { FC, useCallback, useEffect, useState } from "react";
import styled from "styled-components";
import { GlassDiv } from "../styles";
import Flickity from 'react-flickity-component'
import { v4 as uuidv4 } from 'uuid';
import "../styles/flickity-2.css";
import Separator from "./ui/Separator";
import { observer } from "mobx-react";
import { Icon, SearchLink } from "./SearchBar";
import UseStores from "../hooks/useStores";
import { icons } from "../enums";
import { useNavigate } from "react-router-dom";
import dayjs from 'dayjs';

const Slider = styled(Flickity)`
  width: 100%;
  /* height: 100%; */
  display: flex;
  flex-direction: column;
  gap: 4px;
`;

const Description = styled.div<{singlePost?: boolean}>`



  width: -webkit-fill-available;
  font-size: 14px;
  font-weight: 400;
  padding: 0 12px;

  ${(props) => !props.singlePost
    ? "overflow: hidden; white-space: nowrap; text-overflow: ellipsis;"
    : ""
}
`;

const PostContainer = styled(GlassDiv)<{singlePost?: boolean}>`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: start;
  border-radius: 12px;
  padding: 8px;
  gap: 8px;

  width: -webkit-fill-available;
  height: -webkit-fill-available;

  cursor: pointer;

  img {
    width: 100%;
    height: 100%;
    border-radius: 12px;
    ${(props) => !props.singlePost ? (
    "max-height: 29vw; max-width: 30vw;"
  ) : ''}
  }
`

const PostTitle = styled.b`
  font-size: 24px;
`

const Photo = styled.div`
  width: 100%;
  height: min-content;
`

const LikePanel = styled.div`
  display: flex;
  width: 100%;
  justify-content: space-between;
  align-items: center;

  font-weight: 300;

  img {
    width: 48px;
    height: 48px;
  }
`

const BigPostCard: FC<{
  src: string[],
  description?: string,
  profileId?: string,
  singlePost?: boolean,
  date?: string,
  link?: string,
}> = ({src, description, profileId, singlePost, date, link}) => {
  const { userStore } = UseStores();
  const [icon, setIcon] = useState<string | undefined>(undefined);
  const [login, setLogin] = useState('');
  const [formatDate, setFormatDate] = useState(dayjs(date).format('DD-MM-YYYY'));
  const navigate = useNavigate()

  useEffect(() => {
    const getProfile = async () => {
      if (profileId) {
        const profile = await userStore.getPostProfile(profileId);
        setLogin(profile?.profileName ?? '');
        setIcon(profile?.icon.data.profilePhoto ? `data:image/png;base64, ${profile?.icon.data.profilePhoto}` : undefined);
      }
    }

    getProfile();
  }, [profileId])

  useEffect(() => {
    if (date) {
      setFormatDate(dayjs(date).format('DD-MM-YYYY'));
    }
  }, [date])

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
      singlePost={singlePost}
    >
      <SearchLink
        key={uuidv4()}
        onClick={() => {
          navigate(`/profile/${profileId}`)
        }}
      >
        <Icon
          src={icon ? icon : icons.profile}
        />
        <PostTitle>{login}</PostTitle>
      </SearchLink>
      <Separator />
      {MemoSlider(src)}
      <Separator />
      <LikePanel>
        <img src={icons.heart}/>
        {formatDate}
      </LikePanel>
      <Separator />
      <Description
        singlePost={singlePost}
      >
        {description}
      </Description>
    </PostContainer>
  );
};

export default observer(BigPostCard);
