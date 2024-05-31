import { FC } from "react";
import styled from "styled-components";
import { GlassDiv } from "../../styles";

const IconContainer = styled(GlassDiv)`
  padding: 6px;
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
`;

const IconElem = styled.img`
  width: 36px;
  height: 36px;
`;

const Icon: FC<{ icon: string; onClick?: () => void }> = ({
  icon,
  onClick,
}) => {
  return (
    <IconContainer onClick={onClick}>
      <IconElem src={icon} />
    </IconContainer>
  );
};

export default Icon;
