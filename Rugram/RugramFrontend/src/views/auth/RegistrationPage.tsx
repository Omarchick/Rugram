import { createRef, FC, useEffect, useState } from "react";
import Title from "../../components/ui/Title";
import Input from "../../components/ui/Input";
import Button from "../../components/ui/Button";
import {
  FooterContainer,
  LoginContainer,
  Separator,
  StyledLink,
  TextForm,
} from "./AuthStyledComponents";
import UseStores from "../../hooks/useStores";
import { observer } from "mobx-react";
import { WithValidation } from "../../types/commonTypes";

const RegistrationPage: FC = () => {
  const refs = [
    {
      ref: createRef<WithValidation>(),
      id: "TextRef",
    },
  ];

  const [TextRef] = refs;

  const { userStore, modalStore } = UseStores();
  const [email, setEmail] = useState<string>("");
  const [textError, setTextError] = useState<string | undefined>("");

  const checkErrors = () => {
    const errors = [];
    refs.map((ref) => {
      const error = ref.ref.current?.validate();
      if (error) {
        setTextError(error);
        errors.push(error);
      }
    });
    return !!errors.length;
  };

  const registrationHandler = async () => {
    const hasErrors = checkErrors();
    if (!hasErrors) {
      const mailError = await userStore.CheckMail(email)
      if (mailError) {
        userStore.SendEmail(email);
        modalStore.toggleModal();
        modalStore.title = "Проверьте почту!";
        modalStore.content = (
          <p>
            На вашу почту была отправлена ссылка подтверждения для продолжения
            регистрации
          </p>
        );
      } else {
        setTextError('Пользователь с такой почтой уже существует')
      }
    }
  };

  useEffect(() => {
    return () => {
      modalStore.closeModal();
    };
  }, []);

  return (
    <LoginContainer>
      <Title text="Регистрация" />
      <Separator>
        <TextForm>
          <Input
            ref={TextRef.ref}
            errorMessage={textError}
            maxLength={100}
            onChange={(value) => {
              setTextError("");
              setEmail(value);
            }}
            title={"Email"}
            type="email"
            value={email}
          />
          <Button
            onClick={registrationHandler}
            text="Зарегистрироваться"
          />
        </TextForm>
        <FooterContainer>
          <span>Уже есть аккаунт?</span>
          <StyledLink to="/auth/login">Войти</StyledLink>
        </FooterContainer>
      </Separator>
    </LoginContainer>
  );
};

export default observer(RegistrationPage);
