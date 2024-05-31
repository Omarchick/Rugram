import { FC } from 'react';
import styled from 'styled-components';

const Line = styled.div`
  width: -webkit-fill-available;
  padding: 0 24px;
  height: 2px;
  background-color: rgba(0, 0, 0, 0.18);
  box-shadow: 0 4px 16px 0 rgba(0, 0, 0, 0.37);
  /* background: linear-gradient(
    135deg,
    rgba(255, 255, 255, 0.1),
    rgba(255, 255, 255, 0)
  ); */
  backdrop-filter: blur(10px);
  -webkit-backdrop-filter: blur(10px);
  border-radius: 24px;
`

const Separator : FC = () => {
  return (
    <Line />
  );
};

export default Separator;
