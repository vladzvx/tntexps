local cartridge = require('cartridge')
local crud = require('crud')
local vshard = require("vshard")

local yaml = require('yaml')
local ddl = require('ddl')

local role_name = 'my_test_cluster'

--Конфигурирование инстанса тарантула
local function init(opts)
    box.cfg{--в стартовой настройке указываем максимальный размер инстанса в 16 Гб
        memtx_memory= 16 * 1024 * 1024 * 1024
    } 
    if opts.is_master then--если инстанс - мастер - конфигурируем его с помощью ddl схемы (schema.yml)
        local fh = io.open('schema.yml', 'r')
        local schema = yaml.decode(fh:read('*all'))
        fh:close()
        local ok, err = ddl.check_schema(schema)
        if not ok then
            print(err)
        end
        local ok, err = ddl.set_schema(schema)
        if not ok then
            print(err)
        end
    end

end

local function stop()

end


--Экспорт функций
return {
    init = init,
    stop = stop,
    role_name = role_name,
}