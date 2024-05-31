import React, { useEffect, useState } from "react";
import styled from "styled-components";
import { icons } from "../enums";
import { GlassDiv } from "../styles";

interface Props {
  /** Заголовок модального окна */
  title?: React.ReactNode;
  /** Действие на клик по кнопкам закрыть и отмена */
  onClose: (depth?: number) => void;
  /** Дочерние компоненты модального окна */
  children: string | React.ReactNode;
  /** Нижняя часть модального окна */
  footer?: string | React.ReactNode;
  /** вложенность модального окна */
  depth?: number;
  /** Ширина онка */
  width?: number | string;
  /** Без outside клика */
  noOutsideClose?: boolean;
  /** Высота контента */
  height?: number;
  /** Убирает owerflow: auto */
  noScroll?: boolean;
}

const ModalWindowContainer = styled.div<{ depth: number }>`
  position: fixed;
  background-color: rgba(45, 47, 53, 0.1);
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  display: flex;
  align-items: center;
  z-index: ${(props) => 1000 + props.depth};
  justify-content: center;
  height: 100vh;
  padding: 20px;
  > div {
    max-width: 100%;
  }
`;

const Container = styled(GlassDiv)<{
  width?: number | string;
  height?: number;
}>`
  display: flex;
  flex-direction: column;
  border-radius: 16px;
  border: 3px solid black;
  position: relative;
  overflow: hidden;
  width: ${(props) =>
    props.width
      ? `${typeof props.width === "number" ? `${props.width}px` : props.width}`
      : "auto"};
  ${(props) =>
    props.height ? `height: ${props.height}px` : "max-height: 85vh;"};
`;

const TitleHeader = styled.div<{hasTitle?: boolean}>`
  margin-bottom: 24px;
  padding: 42px 46px 0px 46px;
  display: flex;
  justify-content: ${(props) => props.hasTitle ? 'space-between' : 'flex-end' } ;
  align-items: center;
`;

const Body = styled.div<{
  footer: Props["footer"];
  clearPaddings?: boolean;
  noScroll?: boolean;
}>`
  ${(props) => (props.noScroll ? "" : "overflow: auto;")}
  padding: ${(props) =>
    props.footer ? "0 46px 40px 46px" : "0 46px 46px 46px"};
  ${(props) => (props.clearPaddings ? "padding: 0" : undefined)};
  min-height: 100%;
`;

const Footer = styled.div<{ clearPaddings?: boolean }>`
  display: flex;
  padding: ${(p) => (p.clearPaddings ? 0 : "26px 46px")};
  justify-content: end;
  z-index: 100;
`;

const CloseButton = styled.img<Partial<Props>>`
  width: 34px;
  height: 34px;
  z-index: 1000;

  cursor: pointer;
`;

interface DraggingModalStyles {
  left: number;
  top: number;
  position?: "relative" | "absolute";
}

interface DragModalState {
  diffX?: number;
  diffY?: number;
  dragging: boolean;
}

const Title = styled.div`
  font-size: 24px;
  font-weight: 600;
`;

const ModalSearchWindow: React.FC<Props> = ({
  title,
  onClose,
  children,
  footer,
  depth = 0,
  width,
  height,
  noScroll,
}) => {
  useEffect(() => {
    if (depth === 0) {
      window.document.body.style.overflow = "hidden";
    }
    return () => {
      if (depth === 0) {
        window.document.body.style.overflow = "auto";
      }
    };
  });

  const [style, setStyle] = useState<DraggingModalStyles>();

  const [dragState, setDragState] = useState<DragModalState>();

  const [modalParams, setModalParams] = useState<DOMRect>();

  const dragging = (e: React.MouseEvent<HTMLDivElement, MouseEvent>) => {
    if (dragState && dragState.dragging) {
      setStyle({
        left: e.screenX - dragState.diffX!,
        top: e.screenY - dragState.diffY!,
        position: "absolute",
      });
    }
  };

  const dragStop = () => {
    const { height: windowHeight, width: windowWidth } =
      window.document.body.getBoundingClientRect();

    if (style && modalParams) {
      if (modalParams?.left < 0) {
        setStyle({
          ...style,
          left: 10,
        });
      }
      if (modalParams?.top < 0) {
        setStyle({
          ...style,
          top: 10,
        });
      }
      if (Number(modalParams?.left) + modalParams.width > windowWidth) {
        setStyle({
          ...style,
          left: windowWidth - modalParams.width - 10,
        });
      }
      if (Number(modalParams?.top) + modalParams.height > windowHeight) {
        setStyle({
          ...style,
          top: windowHeight - modalParams.height - 10,
        });
      }
    }

    setDragState({
      ...dragState,
      dragging: false,
    });
  };

  return (
    <ModalWindowContainer
      depth={depth}
      onMouseLeave={dragStop}
      onMouseMove={dragging}
    >
      <Container
        height={height}
        onMouseMove={(e: React.MouseEvent<HTMLDivElement, MouseEvent>) =>
          setModalParams(e.currentTarget.getBoundingClientRect())
        }
        style={{
          left: style?.left,
          top: style?.top,
          position: style?.position,
        }}
        width={width}
      >
        <TitleHeader
          hasTitle={!!title}
        >
          {title && (
            <Title>{title}</Title>
          )}
          <CloseButton
            alt="closeButton"
            onClick={() => onClose()}
            src={icons.close}
          />
        </TitleHeader>
        <Body
          footer={footer}
          noScroll={noScroll}
        >
          {children}
        </Body>
        {footer && <Footer>{footer}</Footer>}
      </Container>
    </ModalWindowContainer>
  );
};

export default ModalSearchWindow;
