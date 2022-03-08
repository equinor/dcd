import { ChangeEvent, useState, useEffect } from 'react';
import styled from 'styled-components';
import { language } from '@equinor/eds-icons';
import { Icon, NativeSelect} from '@equinor/eds-core-react';
import { useTranslation } from 'react-i18next';

//TODO: Remove border-color & border-style
const LanguageSelect = styled.div`
    display: flex;
    align-items: center;
`

const LanguageDropdown = styled(NativeSelect)`
    width: 5rem;
    margin-left: 0.5rem;
`

const ChangeLanguageSelect = () => {

    const { t , i18n} = useTranslation();
    const languagesArray = ['nb', 'en']
    const [selectedLanguage, setSelectedLanguage] = useState<String>();

    useEffect(() => {
        const prevLang = localStorage.getItem('CurrentLanguage') || 'en'
        const prevLanguageStringValue = prevLang.toString()
        setSelectedLanguage(prevLanguageStringValue)
      }, []);

    const onSelected = (event: React.ChangeEvent<HTMLSelectElement>) => {
        const selectedLanguage = event.currentTarget.selectedOptions[0].value
        setSelectedLanguage(selectedLanguage) 
        i18n.changeLanguage(selectedLanguage) 
        localStorage.setItem("CurrentLanguage", selectedLanguage)
    }

    return(
        <LanguageSelect>
            <Icon data={language} />
            <LanguageDropdown
                id="select-language"
                label={t('ChangeLanguageSelect.Language')}
                placeholder={'Select a language'}
                onChange={(event: ChangeEvent<HTMLSelectElement>) => onSelected(event)}
            >
                <option value = {selectedLanguage as string} disabled selected={selectedLanguage == null || undefined} />
                {languagesArray.map(language => ( <option value={language} key={language} selected={language === selectedLanguage}>{language}</option>))}
            </LanguageDropdown>
        </LanguageSelect>
                    
    )
}

export default ChangeLanguageSelect;