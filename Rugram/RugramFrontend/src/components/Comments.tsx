import { FC, useState } from 'react';
import styled from 'styled-components';
import { GlassDiv } from '../styles';
import { CommentType } from '../types/commonTypes';
import { icons } from '../enums';
import Separator from './ui/Separator';
import TextArea from './ui/TextArea';

const exampleComments : CommentType[] = [
  
]

type Props = {
  comments?: CommentType[];
}

const CommentsContainer = styled(GlassDiv)`
  width: 100%;
  max-width: 40vw;

  height: 100%;
  display: flex;
  flex-direction: column;
  padding: 24px;
  border-radius: 12px;
  gap: 28px;
`

const CommentContainer = styled.div<{isSameUser: boolean}>`
  display: flex;
  flex-direction: column;
  gap: 4px;


  ${(props) => props.isSameUser
    ? "align-self: flex-end; align-items: flex-end"
    : "align-self: flex-start; align-items: flex-start;"
}
`

const TextContainer = styled(GlassDiv)`
  width: fit-content;
  height: min-content;
  display: flex;
  padding: 8px;
  border-radius: 12px;
`

const Comments : FC<Props> = ({comments = exampleComments}) => {
  const id = '1'
  const [commentState, setCommentState] = useState('');
  return (
    <CommentsContainer>
      <div>
        <h2>Комментарии</h2>
        <Separator />
      </div>
      {comments && comments.map((comment) => (
        <CommentContainer
          key={comment.commentId}
          isSameUser={comment.profileId === id}
        >
          {comment.profileName}
          <TextContainer>
            {comment.commentText}
          </TextContainer>
        </CommentContainer>
      ))}
      <TextArea
        onChange={(value: string) => {
          setCommentState(value)
        }
        }
        placeholder='Напишите ваш комментарий'
        title={''}
        type={'text'}
        value={commentState}
      />
    </CommentsContainer>
  );
};

export default Comments;
