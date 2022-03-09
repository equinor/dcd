import i18n from "i18next"
import { initReactI18next } from "react-i18next"
import LanguageDetector from "i18next-browser-languagedetector"

import translationNB from "./i18n/locales/nb.json"
import translationEN from "./i18n/locales/en.json"

// Implementation based upon documentation from https://react.i18next.com/legacy-v9/step-by-step-guide
const resources = {
    nb: {
        translation: translationNB,
    },
    en: {
        translation: translationEN,
    },
}

i18n
    .use(LanguageDetector)
    .use(initReactI18next)
    .init({
        resources: resources,
        fallbackLng: "en",
        debug: true,

        interpolation: {
            escapeValue: false,
        },
    })

export default i18n
