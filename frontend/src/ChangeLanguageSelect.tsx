import { ChangeEvent, useState } from 'react';
import styled from 'styled-components';
import { search, language } from '@equinor/eds-icons';
import { Icon, NativeSelect} from '@equinor/eds-core-react';
import { useTranslation } from 'react-i18next';


//TODO: Remove border-color & border-style
const LanguageSelect = styled.div`
    border-color: coral; 
    border-style: solid;
    display: flex;
    align-items: center;
`

const LanguageDropdown = styled(NativeSelect)`
    width: 5rem;
    margin-left: 0.5rem;
`

const ChangeLanguageSelect = () => {

    const {t, i18n} = useTranslation();
    const languagesArray = ['nb', 'en']
    const [selectedLanguage, setSelectedLanguage] = useState<String>();

    const onSelected = (event: React.ChangeEvent<HTMLSelectElement>) => {
     
        const selectedLanguage = event.currentTarget.selectedOptions[0].value
        setSelectedLanguage(selectedLanguage) //Visual
        i18n.changeLanguage(selectedLanguage) //Functional
        
        console.log('Current language is: ' + i18n.language)
    }

    return(
        <LanguageSelect>
            <Icon data={language} />
            <LanguageDropdown
                id="select-language"
                label={'Language'}
                placeholder={'Select a language'}
                onChange={(event: ChangeEvent<HTMLSelectElement>) => onSelected(event)}
            >
                <option disabled selected />
                {languagesArray.map(language => ( <option value={language} key={language}>{language}</option>))}
            </LanguageDropdown>
        </LanguageSelect>
                    
    )
}

export default ChangeLanguageSelect;