/* eslint-disable max-len */
import EnvironmentVariables from "../environmentVariables"

/**
 * The cache key (in localStorage) for storing and retrieving the Fusion environment.
 */
export const FUSION_ENV_LOCAL_CACHE_KEY = "CONCEPTAPP_FUSION_ENV"

/**
 * The configuration for the current environment to communicate with the correct backend, using the correct
 * resources etc.
 */
export interface ApiConfig {
    baseApiUrl: string;
    serviceNowApiUrl: string;
    clientId: string;
    scopes: string[];
}

const FusionProductionEnvironmentKey = "FPRD"

const resolveUsingRadixEnvironment = (): ApiConfig => {
    switch (EnvironmentVariables.RADIX_ENVIRONMENT) {
        case "ci":
            return {
                baseApiUrl: "https://backend-dcd-dev.radix.equinor.com",
                serviceNowApiUrl: "https://equinortest.service-now.com",
                clientId: "5dc4598e-e1e8-4669-9a3a-8d2c83471a30",
                scopes: ["api://9b125a0c-4907-43b9-8db2-ff405d6b0524/.default"],
            }
        default:
            throw Error(`Cannot resolve API Configuration, unknown Radix environment "${EnvironmentVariables.RADIX_ENVIRONMENT}"`)
    }
}

const resolveUsingFusionEnvironment = (): ApiConfig => {
    const fusionEnvironment = localStorage.getItem(FUSION_ENV_LOCAL_CACHE_KEY)
    switch (fusionEnvironment) {
        case "CI":
            return {
                baseApiUrl: "https://ase-dcd-backend-dev.azurewebsites.net",
                serviceNowApiUrl: "https://equinortest.service-now.com",
                clientId: "5dc4598e-e1e8-4669-9a3a-8d2c83471a30",
                scopes: ["api://9b125a0c-4907-43b9-8db2-ff405d6b0524/.default"],
            }

        default:
            console.warn(
                "Could not resolve correct environment from the Fusion cache, "
                    + "will fallback to use development. Given Fusion environment was: ",
                fusionEnvironment,
            )
            return {
                // Ideally we would like to use IP directly (127.0.0.1), as fallback to localhost _can_ in certain
                // circumstances resolve to an unintended IP (e.g.the local hosts-file has been modified).
                // HOWEVER, this will not work under local development, as the self-signed certificate is most likely
                // using "localhost" as domain and thus using IP will result in an ERR_CERT_COMMON_NAME_INVALID error
                // during fetch.
                baseApiUrl: "http://localhost:5000",
                serviceNowApiUrl: "https://equinortest.service-now.com",
                clientId: "5dc4598e-e1e8-4669-9a3a-8d2c83471a30",
                scopes: ["api://9b125a0c-4907-43b9-8db2-ff405d6b0524/.default"],
            }
    }
}

export const isProductionEnvironment = (): boolean => {
    const fusionEnvironment = localStorage.getItem(FUSION_ENV_LOCAL_CACHE_KEY)
    return fusionEnvironment === FusionProductionEnvironmentKey
}

/**
 * Resolves the API configuration to use, based on which Fusion environment the application is running in.
 *
 * Normally one would use ENV-variables for this, but this is unfortunately not possible in a Fusion-context;
 * The application is a "child" within the Fusion-framework and does not have access to read the process.env-variables.
 * In addition one can promote an artifact from one environment to the next and using "fixed" environment variables
 * would result in wrong API configuration after promoting.
 */
const resolveConfiguration = (): ApiConfig => (EnvironmentVariables.RADIX_ENVIRONMENT ? resolveUsingRadixEnvironment() : resolveUsingFusionEnvironment())

// Preferably we would export the result of the above function, i.e. "export default resolveConfiguration()",
// rather than the function itself. However, this would lead to the function running directly on import, which again
// makes mocking the localStorage in tests impossible.
// Having it made available as a function is a small tradeoff for testability.
export default resolveConfiguration
