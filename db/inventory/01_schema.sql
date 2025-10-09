CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE IF NOT EXISTS inventory (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    product_id UUID NOT NULL,
    quantity_in_stock INT NOT NULL DEFAULT 0,       -- Số lượng hiện có trong kho
);
