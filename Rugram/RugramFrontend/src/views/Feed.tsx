import { FC, useEffect, useState } from 'react';
import UseStores from '../hooks/useStores';
import styled from 'styled-components';
import BigPostCard from '../components/BigPostCard';
import { observer } from 'mobx-react';
import { v4 as uuidv4 } from 'uuid';
import InfiniteScroll from 'react-infinite-scroll-component';

const FeedPage = styled.div`
  display: flex;
  gap: 36px;
  flex-direction: column;
  justify-content: center;
  min-height: 85vh;
  align-items: center;
  width: 90vw;
`

const Feed : FC = () => {
  const { feedStore, userStore } = UseStores()
  const [pageNumber, setPageNumber] = useState(0);

  useEffect(() => {
    if (userStore.user.id) {
      feedStore.getPosts(10, pageNumber)
    }
  }, [pageNumber])

  useEffect(() => () => {
    feedStore.post = []
  }, [])

  return (
    <FeedPage>
      <InfiniteScroll
        dataLength={feedStore.post.length}
        endMessage={
          <p style={{ textAlign: 'center' }}>
            <b>Больше постов нет!</b>
          </p>
        }
        hasMore={true}
        loader={<h4>Загрузка...</h4>}
        next={() => setPageNumber(pageNumber + 1)}
        style={{ display: 'flex', flexDirection: 'column', gap: '36px', width: '30vw', padding: '1vw 10vw'}}
      >
        { feedStore.post ?
          feedStore.post.map((post) => {
            return (
              <BigPostCard
                key={uuidv4()}
                description={post.description}
                link={post.postId}
                profileId={post.profileId}
                src={post.photoUrls!}
              />
            )
          }) : (<>Тут пока пусто</>)
        }
      </InfiniteScroll>
    </FeedPage>
  )
};

export default observer(Feed);
