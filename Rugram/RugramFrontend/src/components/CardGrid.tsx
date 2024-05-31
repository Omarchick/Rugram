import { FC, useCallback, useEffect, useState } from 'react';
import useStores from '../hooks/useStores';
import styled from 'styled-components';
import { observer } from 'mobx-react';
import PhotoCard from './PhotoCard';
import InfiniteScroll from 'react-infinite-scroll-component';
import { useParams } from 'react-router-dom';

// const Grid = styled.div`
//   width: 100%;
//   display: grid;
//   grid-template-columns: repeat(3, 20vw);
//   gap: 2vw;
//   flex-wrap: wrap;
// `;

const NoPosts = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  font-size: 36px;
  width: 100%;
  min-width: 60vw;
`

const CardGrid : FC = () => {
  const { userStore } = useStores();
  const [pageNumber, setPageNumber] = useState(0);
  const { id } = useParams()

  useEffect(() => {
    if (pageNumber === 0) {
      userStore.user.posts = [];
    }
    if (id) {
      userStore.getPosts(id, pageNumber)
    }
  }, [pageNumber, userStore.user.id, id])

  const hasMore = useCallback(() => {
    return userStore.user.posts?.length !== userStore.user.postsCount;
  }, [userStore.user.posts?.length, userStore.user.postsCount])

  return userStore.user.posts?.length
    ? (
      <InfiniteScroll
        dataLength={userStore.user.posts.length}
        endMessage={<></>}
        hasMore={hasMore()}
        loader={<h4>Загрузка...</h4>}
        next={() => setPageNumber(pageNumber + 1)}
        style={{
          width: '100%',
          display: 'grid',
          gridTemplateColumns: 'repeat(3, 19vw)',
          gap: '1vw',
          flexWrap: 'wrap',
          padding: '1vw'
        }}
      >
        {userStore.user.posts.map((post) => {
          if (post.photoUrls) {
            return (
              <PhotoCard
                key={post.postId}
                description={post.description}
                link={post.postId}
                src={post.photoUrls}
              />
            )
          }
        })}
      </InfiniteScroll>
    ) : (
      <NoPosts> Постов пока нет</NoPosts>
    )

};

export default observer(CardGrid);
