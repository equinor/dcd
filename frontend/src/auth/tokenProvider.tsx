export const obtainAccessToken = async (scopes: string[]): Promise<string | undefined> => {
    console.log("Acquire access token: ")
    // eslint-disable-next-line no-return-await
    return await window.Fusion.modules.auth.acquireAccessToken({ scopes })
}
