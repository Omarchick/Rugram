/* eslint-disable prettier/prettier */
import { createRef, FC, useState } from "react";
import Title from "../../components/ui/Title";
import Input from "../../components/ui/Input";
import Button from "../../components/ui/Button";
import { LoginContainer, Separator, TextForm } from "./AuthStyledComponents";
import UseStores from "../../hooks/useStores";
import { WithValidation } from "../../types/commonTypes";
import { useNavigate } from "react-router-dom";

const ConfirmEmail: FC = () => {
  const refs = [
    {
      ref: createRef<WithValidation>(),
      id: "LoginRef",
    },
    {
      ref: createRef<WithValidation>(),
      id: "PasswordlRef",
    },
    {
      ref: createRef<WithValidation>(),
      id: "RepeatPasswordlRef",
    },
  ];

  const [LoginRef, PasswordRef, RepeatPasswordlRef] = refs;

  const { userStore } = UseStores();
  const navigate = useNavigate();

  const [login, setLogin] = useState<string>("");
  const [loginError, setLoginError] = useState<string | undefined>("");
  const [password, setPassword] = useState<string>("");
  const [passwordError, setPasswordError] = useState<string | undefined>("");
  const [repeatPassword, setRepeatPassword] = useState<string>("");
  const [repeatPasswordError, setRepeatPasswordError] = useState<string | undefined>("");

  const checkErrors = () => {
    const errors = [];
    refs.map((ref) => {
      const error = ref.ref.current?.validate();
      if (error) {
        if (ref.id === "PasswordlRef") {
          setPasswordError(error);
        } else if (ref.id === "LoginRef") {
          setLoginError(error);
        } else {
          setRepeatPasswordError(error);
        }
        errors.push(error);
      }
    });
    if (repeatPassword !== password) {
      setRepeatPasswordError("Пароли не совпадают");
      errors.push("Пароли не совпадают");
    }
    return !!errors.length;
  };

  const registrationHandler = async () => {
    const path = window.location.search.split("=");
    const token = `${path[1]}=`;
    const email = path[3];
    const hasErrors = checkErrors();
    if (!hasErrors) {
      const LoginError = await userStore.CheckProfileName(login);
      if (LoginError) {
        await userStore.Registration(token, email, password, login);
        if (userStore.token) {
          navigate("/feed", { replace: true });
        }
      } else {
        setLoginError("Пользователь с таким именем уже существует");
      }

    }
  };

  return (
    <LoginContainer>
      <Title text="Регистрация" />
      <Separator>
        <TextForm>
          <Input
            ref={LoginRef.ref}
            errorMessage={loginError}
            onChange={(value) => {
              setLoginError("");
              setLogin(value);
            }}
            title={"Имя пользователя"}
            type="login"
            value={login}
          />
          <Input
            ref={PasswordRef.ref}
            errorMessage={passwordError}
            maxLength={16}
            onChange={(value) => {
              setPasswordError("");
              setPassword(value);
            }}
            title={"Пароль"}
            type="password"
            value={password}
          />
          <Input
            ref={RepeatPasswordlRef.ref}
            errorMessage={repeatPasswordError}
            maxLength={16}
            onChange={(value) => {
              setRepeatPasswordError("");
              setRepeatPassword(value);
            }}
            title={"Повторите пароль"}
            type="password"
            value={repeatPassword}
          />
          <Button
            onClick={registrationHandler}
            text="Зарегистрироваться"
          />
        </TextForm>
      </Separator>
    </LoginContainer>
  );
};

export default ConfirmEmail;
