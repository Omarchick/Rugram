import { FC, useEffect } from "react";
import BackgroundVideo from "./BackgroundVideo";
import { Outlet, useNavigate } from "react-router-dom";
import LogoComponent from "./LogoComponent";
import styled from "styled-components";
import { observer } from "mobx-react";
import UseStores from "../hooks/useStores";
import ModalWindow from "./ModalWindow";

const Container = styled.div`
  padding: 36px 48px;
  display: flex;
  flex-direction: row;
  gap: 48px;
`;

const TextContainer = styled.div`
  width: 50vw;
  height: 90vh;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
`;

const MarketingPhrases = styled.p`
  width: 60%;
  padding: 16px 32px;
  border: 4px black solid;
  display: flex;
  font-size: 28px;
  font-weight: 700;
  &:nth-child(2n) {
    align-self: flex-end;
  }
`;

const AuthLayout: FC = () => {
  const navigate = useNavigate();
  const { modalStore, userStore } = UseStores();
  useEffect(() => {
    if (window.location.pathname === "/" && !userStore.token) {
      navigate("/auth/login");
    }
    if (userStore.token) {
      navigate('/feed')
    }
  }, [window.location]);

  useEffect(() => {
    return () => {
      modalStore.clearStore();
    };
  }, []);


  return (
    <Container>
      <BackgroundVideo />
      <LogoComponent />
      <Outlet />
      <TextContainer>
        <MarketingPhrases>
          Делитесь кусочком своей жизни со всеми
        </MarketingPhrases>
        <MarketingPhrases>Следите за обновлениями друзей</MarketingPhrases>
        <MarketingPhrases>Узнавайте новые места</MarketingPhrases>
        <MarketingPhrases>Воплощай свои мечты в социальной сети</MarketingPhrases>
      </TextContainer>

      {modalStore.isOpen && (
        <ModalWindow
          onClose={() => {
            modalStore.toggleModal();
          }}
          title={modalStore.title}
        >
          {modalStore.content}
        </ModalWindow>
      )}
    </Container>
  );
};

export default observer(AuthLayout);
