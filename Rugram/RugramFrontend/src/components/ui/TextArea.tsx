import { forwardRef, useImperativeHandle } from "react";
import styled from "styled-components";
import { WithValidation } from "../../types/commonTypes";
import TextareaAutosize from 'react-textarea-autosize';

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

const StyledTextArea = styled(TextareaAutosize)<{ error: boolean }>`
  padding: 16px;
  border-radius: 8px;
  width: -webkit-fill-available;
  font-size: 16px;
  min-height: 20px;

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
  resize: none;
  opacity: 1;
  overflow: auto;
  line-height: 20px;
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

const TextArea = forwardRef<WithValidation, Props>(
  (
    {
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
        return undefined;
      },
    }));

    return (
      <InputContainer>
        <Title>{title}</Title>
        <StyledTextArea
          error={!!errorMessage}
          onChange={(newValue) => onChange(newValue.target.value)}
          placeholder={placeholder}
          value={value}
        />
        {errorMessage && <ErrorContainer>{errorMessage}</ErrorContainer>}
      </InputContainer>
    );
  },
);

TextArea.displayName = "TextArea";
export default TextArea;
