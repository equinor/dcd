// eslint-disable-next-line max-len
// Unfortunately it is very hard to mock this using jest, thus keeping this file as small as possible as it does not
// have any test-coverage.
declare const RADIX_ENVIRONMENT: string

/**
 * The Environment variables for the entire application.
@@ -15,6 +16,7 @@ declare const API_URL : string;
 */
const EnvironmentVariables = {
    RADIX_ENVIRONMENT: typeof RADIX_ENVIRONMENT !== "undefined" ? RADIX_ENVIRONMENT : "",
}

export default EnvironmentVariables
