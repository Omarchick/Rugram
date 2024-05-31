import { forwardRef, useImperativeHandle } from "react";
import styled from "styled-components";
import { regexp } from "../../utils";
import { WithValidation } from "../../types/commonTypes";

type Props = {
  type: "text" | "password" | "email" | "login";
  value: string;
  onChange: (value: string) => void;
  title: string;
  placeholder?: string;
  errorMessage?: string;
  maxLength?: number;
  minLength?: number;
};

const StyledInput = styled.input<{ error: boolean }>`
  padding: 16px;
  border-radius: 8px;
  width: -webkit-fill-available;
  font-size: 16px;

  background: linear-gradient(
    135deg,
    rgba(255, 255, 255, 0.1),
    rgba(255, 255, 255, 0)
  );
  backdrop-filter: blur(10px);
  -webkit-backdrop-filter: blur(10px);
  border: ${(props) =>
    props.error ? "1px solid red" : "1px solid rgba(255, 255, 255, 0.18)"};
  box-shadow: 0 8px 32px 0 rgba(0, 0, 0, 0.37);

  &:focus {
    outline: none;
  }
`;

const InputContainer = styled.div`
  display: flex;
  flex-direction: column;
  gap: 12px;
  align-items: flex-start;
  justify-content: center;
  box-sizing: border-box;
`;

const Title = styled.div`
  font-size: 20px;
  font-weight: 600;
`;

const ErrorContainer = styled.div`
  color: red;
  font-size: 12px;
  margin: 0;
`;

const Input = forwardRef<WithValidation, Props>(
  (
    {
      type,
      value,
      onChange,
      title,
      placeholder,
      errorMessage,
      maxLength = 25,
      minLength = 5,
    },
    ref,
  ) => {
    useImperativeHandle(ref, () => ({
      validate() {
        if (value.length === 0) {
          return "Поле обязательно для заполнения";
        }
        if (value.length > maxLength) {
          return `Максимальная длина ${maxLength} символов`;
        }
        if (value.length < minLength) {
          return `Минимальная длина ${minLength} символов`;
        }
        if (type === "email" && !regexp.email.test(value)) {
          return "Некоректный email";
        }
        if (type === "password" && !regexp.password.test(value)) {
          return "Пароль должен содержать цифры, буквы и спецсимволы";
        }
        if (type === "login" && !regexp.login.test(value)) {
          return "Логин может содержать только буквы латинского алфавита, цифры и спец. символы";
        }
        return undefined;
      },
    }));

    return (
      <InputContainer>
        <Title>{title}</Title>
        <StyledInput
          error={!!errorMessage}
          onChange={(newValue) => onChange(newValue.target.value)}
          placeholder={placeholder}
          type={type}
          value={value}
        />
        {errorMessage && <ErrorContainer>{errorMessage}</ErrorContainer>}
      </InputContainer>
    );
  },
);

Input.displayName = "Input";
export default Input;
