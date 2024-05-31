import { FC, useEffect } from "react";
import styled from "styled-components";
import BigPostCard from "../components/BigPostCard";
import UseStores from "../hooks/useStores";
import { useParams } from "react-router-dom";
import { observer } from "mobx-react";
import Comments from "../components/Comments";

const PostPageContainer = styled.div`
  display: flex;
  gap: 36px;
  justify-content: start;
  min-height: 85vh;
  width: 90vw;
  margin-left: 84px;
`

const PostContainer = styled.div`
  display: flex;
  max-width: 30vw;
  width: 100%;
  border-radius: 16px;

  img {
    max-width: 30vw;
    max-height: 30vw;
  }
`

const PostPage: FC = () => {
  const { userStore } = UseStores();
  const { id } = useParams();

  useEffect(() => {
    if (id) {
      userStore.getSinglePost(id)
    }
  }, [])

  useEffect(() => {
    return () => {
      userStore.singlePost = undefined;
    }
  }, []);

  return (
    <PostPageContainer>
      <PostContainer>
        {
          userStore.singlePost && (
            <BigPostCard
              date={userStore.singlePost.dateOfCreation}
              description={userStore.singlePost?.description}
              profileId={userStore.singlePost?.profileId}
              src={(userStore.singlePost?.photos ?? [])}
              singlePost
            />
          )
        }
      </PostContainer>
      <Comments />
    </PostPageContainer>

  );
};

export default observer(PostPage);
