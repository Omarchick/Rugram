import { FC } from "react";
import { icons } from "../enums";
import styled from "styled-components";

const Logo = styled.div`
  position: fixed;
  right: 30px;
  z-index: 1000;
  img {
    height: 48px;
  }
`;

const LogoComponent: FC = () => {
  return (
    <Logo>
      <img src={icons.logo} />
    </Logo>
  );
};

export default LogoComponent;
