import { Link } from "react-router-dom";
import styled from "styled-components";
import { GlassDiv } from "../../styles";

export const LoginContainer = styled(GlassDiv)`
  /* height: 60vh; */
  width: 25vw;
  display: flex;
  flex-direction: column;
  align-self: center;
  padding: 36px 36px;
  border-radius: 16px;
`;

export const FooterContainer = styled.div`
  display: flex;
  align-items: center;
  justify-content: center;
  flex-direction: column;
  margin-top: 10vh;
`;

export const StyledLink = styled(Link)`
  color: blue;
  text-decoration: none;
`;

export const Separator = styled.div`
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  justify-self: center;
  height: 100%;
  margin-top: 20%;
`;

export const TextForm = styled.div`
  display: flex;
  flex-direction: column;
  gap: 24px;
`;
