import { Dispatch, SetStateAction } from "react"
import { useLocation, useNavigate, useParams } from "react-router"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"
import { IAsset } from "../models/assets/IAsset"
import { Project } from "../models/Project"
import { IAssetService } from "../Services/IAssetService"
import { EMPTY_GUID } from "../Utils/constants"
import {
    SaveButton,
} from "../Views/Asset/StyledAssetComponents"

interface Props {
    name: string
    setHasChanges: Dispatch<SetStateAction<boolean>>
    hasChanges: boolean
    setAsset: Dispatch<SetStateAction<any | undefined>>
    setProject: Dispatch<SetStateAction<Project | undefined>>
    asset: IAsset
    assetService: IAssetService
    assetType: AssetTypeEnum
}

const Save = ({
    name,
    setHasChanges,
    hasChanges,
    setAsset,
    setProject,
    asset,
    assetService,
    assetType,
}: Props) => {
    const params = useParams()
    const navigate = useNavigate()
    const location = useLocation()

    const handleSave = async () => {
        const assetDto: IAsset = { ...asset }
        assetDto.name = name
        if (asset?.id === EMPTY_GUID) {
            assetDto.projectId = params.projectId
            const newProject = await assetService.create(params.caseId!, assetDto!)
            const newAsset = newProject[assetType].at(-1)
            const newUrl = location.pathname.replace(EMPTY_GUID, newAsset!.id!)
            navigate(`${newUrl}`)
            setProject(newProject)
            setAsset(newAsset)
        } else {
            const newProject = await assetService.update(assetDto!)
            setProject(newProject)
        }
        setHasChanges(false)
    }

    return (
        <SaveButton disabled={!hasChanges || name === ""} onClick={handleSave}>
            Save
        </SaveButton>
    )
}

export default Save
