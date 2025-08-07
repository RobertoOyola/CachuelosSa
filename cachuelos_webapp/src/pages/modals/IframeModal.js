
const IframeModal = ({ show, onClose, src, title }) => {

    return (
        show && (
            <div className="modal modal-xl show" style={{ display: 'block' }}>
                <div className="modal-dialog modal-dialog-scrollable">
                    <div className="container-fluid">
                        <div className="modal-content">
                            <div className="modal-header row">
                                <button type="button" className="btn btn-secondary btn-sm col-1" onClick={onClose}>
                                    &times;
                                </button>
                            </div>
                            <div className="modal-body row">
                                <iframe
                                    title={title}
                                    src={src}
                                    style={{
                                        width: '100%',
                                        height: '80vh',
                                        border: 'none',
                                    }} />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    )
}
export default IframeModal;
