import { FC } from 'react';
import styled from 'styled-components';
import { GlassDiv } from '../styles';
import { observer } from 'mobx-react';

type Props = {
  error?: string;
}

const Modal = styled(GlassDiv)`
  position: absolute;
  padding: 12px;
  border-radius: 12px;
  bottom: 5px;
  right: 5px;
  left: 5px;
  color: red;
  border: 2px solid red;
`

const ErrorModal : FC<Props> = ({ error }) => {
  return (
    <Modal>
      {error && (<>{error}</>)}
    </Modal>
  );
};

export default observer(ErrorModal);
