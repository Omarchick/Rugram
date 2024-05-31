/* eslint-disable prettier/prettier */
import { createRef, FC, useState } from "react";
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
import { WithValidation } from "../../types/commonTypes";
import { useNavigate } from "react-router-dom";
import UseStores from "../../hooks/useStores";

const Login: FC = () => {
  const refs = [
    {
      ref: createRef<WithValidation>(),
      id: "EmailRef",
    },
    {
      ref: createRef<WithValidation>(),
      id: "PasswordlRef",
    },
  ];

  const [EmailRef, PasswordRef] = refs;

  const { userStore } = UseStores();
  const navigate = useNavigate();

  const [email, setEmail] = useState<string>("");
  const [emailError, setEmailError] = useState<string | undefined>("");
  const [password, setPassword] = useState<string>("");
  const [passwordError, setPasswordError] = useState<string | undefined>("");

  const checkErrors = () => {
    const errors: (string | undefined)[] = [];
    refs.map((ref) => {
      const error = ref.ref.current?.validate();
      if (error) {
        if (ref.id === "EmailRef") {
          setEmailError(error);
        } else {
          setPasswordError(error);
        }
        errors.push(error);
      }
    });
    return !!errors.length;
  };

  const registrationHandler = async () => {
    if (!checkErrors()) {
      await userStore.Login(email, password);
      if (userStore.token) {
        navigate('/feed');
      } else {
        setEmailError('Неверная почта или пароль');
        setPasswordError('Неверная почта или пароль');
      }

    }
  };

  return (
    <LoginContainer>
      <Title text="Вход" />
      <Separator>
        <TextForm>
          <Input
            ref={EmailRef.ref}
            errorMessage={emailError}
            maxLength={100}
            onChange={(value) => {
              setEmailError("");
              setEmail(value);
            }}
            title={"Email"}
            type="email"
            value={email}
          />
          <Input
            ref={PasswordRef.ref}
            errorMessage={passwordError}
            onChange={(value) => {
              setPasswordError("");
              setPassword(value);
            }}
            title={"Пароль"}
            type="password"
            value={password}
          />
          <Button
            onClick={registrationHandler}
            text="Войти"
          />
        </TextForm>
        <FooterContainer>
          <span>Ещё нет аккаунта?</span>
          <StyledLink to="/auth/registration">Регистрация</StyledLink>
        </FooterContainer>
      </Separator>
    </LoginContainer>
  );
};

export default Login;
