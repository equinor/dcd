import axios, { AxiosInstance, AxiosRequestConfig, ResponseType } from "axios"

import { config } from "./config"

import { dateStringToDateUtc } from "@/Utils/DateUtils"

const getToken = (): Promise<string | undefined> => {
    const scopes = [[window.sessionStorage.getItem("appScope") || ""][0]]

    return window.Fusion.modules.auth.acquireAccessToken({ scopes })
}

const getLastForcedReloadDate = (): string | null => window.localStorage.getItem("forcedReloadDate")

const setLastForcedReloadDate = (date: string): void => window.localStorage.setItem("forcedReloadDate", date)

type RequestOptions = {
    credentials?: RequestCredentials
    headers?: Record<string, string>
    method?: "GET"
    | "DELETE"
    | "HEAD"
    | "OPTIONS"
    | "POST"
    | "PUT"
    | "PATCH"
    | "PURGE"
    | "LINK"
    | "UNLINK"
    body?: Record<string, any>
}
export class __BaseService {
    private client: AxiosInstance

    constructor() {
        this.client = axios.create({ baseURL: config.BaseUrl.BASE_URL })
        this.client.interceptors.response.use(
            (response: any) => response,
            (error: any) => {
                if (error.response.status === 403) {
                    __BaseService.handle403Error(error)
                } else if (error.response.status === 500) {
                    console.error("Error: An internal server error occurred. Please try again later.")
                }

                return Promise.reject(error)
            },
        )
    }

    private async setAuthHeaders(): Promise<void> {
        const token = await getToken()

        if (!token) {
            throw new Error("Failed to acquire access token")
        }

        this.client.defaults.headers.common = {
            Accept: "application/json",
            Authorization: `Bearer ${token}`,
        }
    }

    private async request(path: string, options?: RequestOptions): Promise<any> {
        await this.setAuthHeaders()

        const { data } = await this.client.request({
            method: options?.method,
            headers: options?.headers,
            withCredentials: options?.credentials === "include",
            url: path,
            data: options?.body,
        })

        return data
    }

    private async requestExcel(path: string, responseType?: ResponseType, options?: RequestOptions): Promise<any> {
        await this.setAuthHeaders()

        const { data } = await this.client.request({
            method: options?.method,
            headers: options?.headers,
            withCredentials: options?.credentials === "include",
            url: path,
            data: options?.body,
            responseType: responseType ?? "text",
        })

        return data
    }

    protected async postWithParams(
        path: string,
        options?: RequestOptions,
        requestQuery?: AxiosRequestConfig,
    ): Promise<any> {
        await this.setAuthHeaders()

        const { data } = await this.client.post(path, options?.body, requestQuery)

        return data
    }

    protected async putWithParams(
        path: string,
        options?: RequestOptions,
        requestQuery?: AxiosRequestConfig,
    ): Promise<any> {
        await this.setAuthHeaders()

        const { data } = await this.client.put(path, options?.body, requestQuery)

        return data
    }

    protected async getWithParams(
        path: string,
        requestQuery?: AxiosRequestConfig,
    ): Promise<any> {
        await this.setAuthHeaders()

        const { data } = await this.client.get(path, requestQuery)

        return data
    }

    protected postExcel<T = any>(path: string, responseType?: ResponseType, options?: RequestOptions): Promise<T> {
        return this.requestExcel(path, responseType, { ...options, method: "POST" })
    }

    protected get<T = any>(path: string, options?: RequestOptions): Promise<T> {
        return this.request(path, { ...options, method: "GET" })
    }

    protected post<T = any>(path: string, options?: RequestOptions): Promise<T> {
        return this.request(path, { ...options, method: "POST" })
    }

    protected delete<T = any>(path: string, options?: RequestOptions): Promise<T> {
        return this.request(path, { ...options, method: "DELETE" })
    }

    protected put<T = any>(path: string, options?: RequestOptions): Promise<T> {
        return this.request(path, { ...options, method: "PUT" })
    }

    private static handle403Error = (error: any) => {
        console.error("Error: You don't have permission to access this resource. Please contact support.")
        const lastForcedReloadDate = getLastForcedReloadDate()
        const reloadTimeoutMs = 5 * 60 * 1000
        const isPastReloadTimeout = !lastForcedReloadDate || new Date().valueOf() - dateStringToDateUtc(lastForcedReloadDate).valueOf() > reloadTimeoutMs
        const expectedAuth403Data = ""

        if (error.response.data === expectedAuth403Data && isPastReloadTimeout) {
            setLastForcedReloadDate(new Date().toISOString())
            window.location.reload()
        }
    }
}
