import { FC } from "react";
import styled from "styled-components";
import { icons } from "../enums";
import { NavLink } from "react-router-dom";
import { GlassDiv } from "../styles";
import Icon from "./ui/Icon";
import UseStores from "../hooks/useStores";
import { observer } from "mobx-react";

const NavBar = styled.div`
  height: 100%;
`

const BarContainer = styled(GlassDiv)`
  display: flex;
  flex-direction: column;
  position: fixed;
  padding: 16px 8px;
  justify-content: space-between;
  min-height: 88vh;
  border-radius: 10px;
`;

const LinkElem = styled(NavLink)`
  &.active {
    img {
      filter: invert(45%) sepia(16%) saturate(1218%) hue-rotate(204deg)
        brightness(104%) contrast(95%);
    }
  }
`;

const MainButtons = styled.div`
  display: flex;
  flex-direction: column;
  gap: 16px;
`;

const Navbar: FC = () => {
  const {userStore} = UseStores();

  return (
    <NavBar>
      <BarContainer>
        <MainButtons>
          <LinkElem to={"/feed"}>
            <Icon icon={icons.browse} />
          </LinkElem>
          <LinkElem to={`/profile/${userStore.user.id}`}>
            <Icon icon={icons.profile} />
          </LinkElem>
          <LinkElem to={"/add"}>
            <Icon icon={icons.plus} />
          </LinkElem>
        </MainButtons>
        <Icon
          icon={icons.exit}
          onClick={() => userStore.LogOut()}
        />
      </BarContainer>

    </NavBar>
  );
};

export default observer(Navbar);
