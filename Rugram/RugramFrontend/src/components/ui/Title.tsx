import { FC } from "react";
import styled from "styled-components";

const TitleContainer = styled.div`
  font-size: 36px;
  font-weight: bold;
`;

const Title: FC<{ text: string }> = ({ text }) => {
  return <TitleContainer>{text}</TitleContainer>;
};

export default Title;
