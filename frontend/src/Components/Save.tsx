import { useLocation, useNavigate, useParams } from "react-router"
import { IAsset } from "../models/assets/IAsset"
import { Project } from "../models/Project"
import { EMPTY_GUID } from "../Utils/constants"
import {
    SaveButton, Wrapper,
} from "../Views/Asset/StyledAssetComponents"

interface IAssetService {
    create: (caseId: string, asset: IAsset) => Promise<Project>
    update: (asset: IAsset) => Promise<Project>
}

interface Props {
    name: string
    setHasChanges: React.Dispatch<React.SetStateAction<boolean>>
    hasChanges: boolean
    setAsset: React.Dispatch<React.SetStateAction<any | undefined>>
    setProject: React.Dispatch<React.SetStateAction<Project | undefined>>
    asset: IAsset
    assetService: IAssetService
}

const Save = ({
    name,
    setHasChanges,
    hasChanges,
    setAsset,
    setProject,
    asset,
    assetService,
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
            const newAsset = newProject.substructures.at(-1)
            const newUrl = location.pathname.replace(EMPTY_GUID, newAsset!.id!)
            navigate(`${newUrl}`)
            setAsset(newAsset)
        } else {
            const newProject = await assetService.update(assetDto!)
            setProject(newProject)
        }
        setHasChanges(false)
    }

    return (
        <Wrapper>
            <SaveButton disabled={!hasChanges} onClick={handleSave}>
                Save
            </SaveButton>
        </Wrapper>
    )
}

export default Save
