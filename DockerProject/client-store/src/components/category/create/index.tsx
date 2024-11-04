import {ICategoryCreate, IUploadedFile} from "./types.ts";
import {Button, Form, Input, Modal, Row, Upload, UploadFile} from "antd";
import {Link, useNavigate} from "react-router-dom";
import {useState} from "react";
import {PlusOutlined} from '@ant-design/icons';
import {RcFile, UploadChangeParam} from "antd/es/upload";
import {http_common} from "../../../env";

const CategoryCreatePage = () => {

    const navigate = useNavigate();
    const [form] = Form.useForm<ICategoryCreate>();

    const [previewOpen, setPreviewOpen] = useState<boolean>(false);
    const [previewImage, setPreviewImage] = useState('');
    const [previewTitle, setPreviewTitle] = useState('');

    const onSubmit = async(values: ICategoryCreate) => {
        console.log("Send Data", values);
        http_common.post<ICategoryCreate>("/api/categories", values,
            {headers: {"Content-Type": "multipart/form-data"}})
            .then(resp => {
                console.log("Craete category", resp.data);
                navigate('/');
            })
    }

    return (
        <>
            <h1 className={"text-center text-3xl font-bold tracking-tight text-gray-900 mb-2"}>Додати категорії</h1>

            <Form
                form={form}
                onFinish={onSubmit}
                layout="vertical">

                <Form.Item
                    label={"Назва"}
                    name={"name"}
                    htmlFor={"name"}
                    rules={[
                        {required: true, message: "Це поле є обов'язковим!"},
                        {min: 3, message: "Довжина поля 3 символи"}
                    ]}
                >
                    <Input autoComplete="name"
                           className={"block w-full px-5  text-gray-700 placeholder-gray-400 bg-white border border-gray-200 rounded-lg dark:placeholder-gray-600 dark:bg-gray-900 dark:text-gray-300 dark:border-gray-700 focus:border-blue-400 dark:focus:border-blue-400 focus:ring-blue-400 focus:outline-none focus:ring focus:ring-opacity-40"}/>
                </Form.Item>

                <div className="flex items-center col-span-2 gap-x-2">
                    <Form.Item
                        name="image"
                        label="Фото"
                        valuePropName="image"
                        getValueFromEvent={(e: UploadChangeParam) => {
                            const image = e?.fileList[0] as IUploadedFile;
                            return image?.originFileObj;
                        }}
                        // rules={[{required: true, message: 'Оберіть фото категорії!'}]}
                    >
                        <Upload
                            // showUploadList={{showPreviewIcon: false}}
                            beforeUpload={() => false}
                            accept="image/*"
                            onPreview={(file: UploadFile) => {
                                if (!file.url && !file.preview) {
                                    file.preview = URL.createObjectURL(file.originFileObj as RcFile);
                                }

                                setPreviewImage(file.url || (file.preview as string));
                                setPreviewOpen(true);
                                setPreviewTitle(file.name || file.url!.substring(file.url!.lastIndexOf('/') + 1));
                            }}
                            listType="picture-card"
                            maxCount={1}
                        >
                            <div>
                                <PlusOutlined/>
                                <div style={{marginTop: 8}}>Upload</div>
                            </div>
                        </Upload>
                    </Form.Item>
                </div>


                <Row style={{display: 'flex', justifyContent: 'center'}}>
                    <Button style={{margin: 10}}
                            className={"text-white capitalize transition-colors duration-300 transform bg-blue-500"}
                            type="primary" htmlType="submit">
                        Додати
                    </Button>
                    <Link to={"/"}>
                        <Button style={{margin: 10}} htmlType="button">
                            Скасувати
                        </Button>
                    </Link>
                </Row>

            </Form>

            <Modal open={previewOpen} title={previewTitle} footer={null} onCancel={() => setPreviewOpen(false)}>
                <img alt="example" style={{width: '100%'}} src={previewImage}/>
            </Modal>
        </>
    )
}

export default CategoryCreatePage;