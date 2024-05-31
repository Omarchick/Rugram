import { FC, useEffect, useState } from "react";
import styled from "styled-components";
import { icons } from "../enums";
import { GlassDiv } from "../styles";
import useDebounce from "../hooks/useDebounce";
import UseStores from "../hooks/useStores";
import { useNavigate } from "react-router-dom";
import { observer } from "mobx-react";

const SearchContainer = styled.div`
  min-width: 88vw;
  display: flex;
  justify-content: center;
  position: fixed;
  top: 20px;
  z-index: 999;
  align-self: center;
  width: -webkit-fill-available;
  margin-left: 69px;
`;

const BarContainer = styled(GlassDiv)`
  display: flex;
  width: 40vw;
  padding: 6px 12px;
  align-items: center;
  border-radius: 12px;
  gap: 12px;
  img {
    width: 24px;
    height: 24px;
  };
`;

const StyledInput = styled.input`
  background-color: inherit;
  border: none;
  width: 90%;
  padding: 8px;
  font-size: 16px;
  &:focus {
    outline: none;
  };
`;

const SearchResult = styled(GlassDiv)`
  display: flex;
  position: absolute;
  flex-direction: column;
  gap: 8px;
  padding: 6px 12px;
  border-radius: 12px;
  z-index: 999;
  top: 64px;
  width: 40vw;
  align-items: center;
`;

export const SearchLink = styled.div`
  display: flex;
  justify-content: left;
  align-items: center;
  padding: 6px 12px;
  width: 100%;
  /* justify-content: center; */
  gap: 24px;
  cursor: pointer;
  font-size: 18px;
`

export const Icon = styled.img`
  /* background-color: red; */
  padding: 2px !important;
  width: 48px !important;
  height: 48px !important;
  border-radius: 50% !important;
  border: 1px solid black !important;

`

const SearchBar: FC = () => {
  const { userStore } = UseStores();
  const [search, setSearch] = useState("");
  const debounceSearch = useDebounce(search, 1000);
  const navigate = useNavigate();

  const target = document.querySelector('#myTarget')

  document.addEventListener('click', (event) => {
    // eslint-disable-next-line @typescript-eslint/ban-ts-comment
    // @ts-ignore
    const withinBoundaries = event.composedPath().includes(target)

    if (!withinBoundaries) {
      userStore.searchProfiles = undefined;
    }
  })

  useEffect(() => {
    if (search) {
      userStore.search(search);
    }
  }, [debounceSearch]);

  return (
    <SearchContainer id='myTarget'>
      <BarContainer
        onClick={() => {
          userStore.search('')
        }}
      >
        <img src={icons.search} />
        <StyledInput
          onChange={(value) => setSearch(value.target.value)}
          placeholder="Начните вводить логин"
          type="text"
          value={search}
        />
      </BarContainer>
      {!!userStore.searchProfiles?.profiles && (
        <SearchResult>
          {userStore.searchProfiles?.profiles.length
            ? userStore.searchProfiles?.profiles.map(
              (profile) => (
                <SearchLink
                  key={profile.id}
                  onClick={() => {
                    setSearch("");
                    userStore.searchProfiles = undefined;
                    navigate(`/profile/${profile.id}`);
                  }}
                >
                  <Icon
                    src={profile.profileImg ? `data:image/png;base64, ${profile.profileImg}` : icons.profile}
                  />
                  {profile.profileName}
                </SearchLink>
              )
            ) : (
              <div>No results</div>
            )}
        </SearchResult>

      )}
    </SearchContainer>
  );
};

export default observer(SearchBar);
