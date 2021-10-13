import { Stream } from '../Stream';
/**************************************** 数据配置表 *****************************************/
/** AllClass表数据类 */
export class AllClassCfg extends Stream {
	static readonly relative = 'alltype/allclass.da';
	static readonly refence = 'AllTypeAllClassCfg';
	private _cfgs: AllClass[] = [];
	constructor(rootDir: string) {
		super(rootDir + AllClassCfg.relative);
	}
	Get(id: number): AllClass | undefined {
		if (this.hasLoaded) {
			return this._cfgs[id];
		}
		else {
			this.LoadConfig();
			return this._cfgs[id];
		}
	}
	protected ParseConfig() {
		this._cfgs = this.GetList('AllTypeAllClass');
	}
	[Symbol.iterator]() { return this._cfgs.values(); }
}
/** CheckAll表数据类 */
export class CheckAllCfg extends Stream {
	static readonly relative = 'alltype/checkall.da';
	static readonly refence = 'AllTypeCheckAllCfg';
	private _cfgs: CheckAll[] = [];
	constructor(rootDir: string) {
		super(rootDir + CheckAllCfg.relative);
	}
	Get(id: number): CheckAll | undefined {
		if (this.hasLoaded) {
			return this._cfgs[id];
		}
		else {
			this.LoadConfig();
			return this._cfgs[id];
		}
	}
	protected ParseConfig() {
		this._cfgs = this.GetList('AllTypeCheckAll');
	}
	[Symbol.iterator]() { return this._cfgs.values(); }
}
export let _CFG_CLASS_ = [AllClassCfg, CheckAllCfg];

/**************************************** 数据结构定义 *****************************************/
/** 卡牌枚举 */
export enum CardElement{
	/**攻击*/
	Attack = 0,
	/**抽牌*/
	Extract = 1,
	/**弃牌*/
	Renounce = 2,
	/**护甲*/
	Armor = 3,
	/**控制*/
	Control = 4,
	/**治疗*/
	Cure = 5,
	/**自残*/
	Oneself = 6,
	/**手牌*/
	Hand = 7,
	/**牌库*/
	Brary = 8,
	/**手牌攻击*/
	Handack = 9,
}

/** 所有类型 */
export class AllClass {
	/** 常量字符串 */
	static readonly ItemString = 'Hello World';
	/** 常量浮点值 */
	static readonly ItemFloat = 3.141527;
	/** 常量布尔值 */
	static readonly ItemBool = false;
	/** 常量枚举值 */
	static readonly ItemEnum = CardElement.Renounce;
	/** ID */
	readonly ID: number;
	/** Test.TID */
	readonly Index: number;
	/** 长整型 */
	readonly VarLong: number;
	/** 浮点型 */
	readonly VarFloat: number;
	/** 字符串 */
	readonly VarString: string;
	/** 布尔型 */
	readonly VarBool: boolean;
	/** 枚举类型 */
	readonly VarEnum: CardElement;
	/** 类类型 */
	readonly VarClass: SingleClass;
	/** 字符串列表 */
	readonly VarListBase: string[];
	/** Class列表 */
	readonly VarListClass: SingleClass[];
	/** 字符串列表 */
	readonly VarListCardElem: string[];
	/** 浮点数列表 */
	readonly VarListFloat: number[];
	/** 基础类型字典 */
	readonly VarDictBase: Map<number, number>;
	/** 枚举类型字典 */
	readonly VarDictEnum: Map<number, string>;
	/** 类类型字典 */
	readonly VarDictClass: Map<string, SingleClass>;
	constructor(stream: any) {
		this.ID = stream.GetInt();
		this.Index = stream.GetInt();
		this.VarLong = stream.GetLong();
		this.VarFloat = stream.GetFloat();
		this.VarString = stream.GetString();
		this.VarBool = stream.GetBool();
		this.VarEnum = <CardElement> stream.GetInt();
		this.VarClass = stream.GetAllTypeSingleClassMaker();
		this.VarListBase = stream.GetList('String');
		this.VarListClass = stream.GetList('AllTypeSingleClassMaker');
		this.VarListCardElem = stream.GetList('String');
		this.VarListFloat = stream.GetList('Float');
		this.VarDictBase = stream.GetDict('Int', 'Float');
		this.VarDictEnum = stream.GetDict('Long', 'String');
		this.VarDictClass = stream.GetDict('String', 'AllTypeSingleClassMaker');
	}
}
Object.defineProperty(Stream.prototype, 'GetAllTypeAllClass', {
	value: (stream: any) => new AllClass(stream),
	writable: false,
});
/** 所有类型 */
export class CheckAll {
	/** ID */
	readonly ID: number;
	/** Test.TID */
	readonly Index: number;
	/** 长整型 */
	readonly VarLong: number;
	/** 浮点型 */
	readonly VarFloat: number;
	/** 字符串 */
	readonly VarString: string;
	/** 字符串列表 */
	readonly VarListString: string[];
	/** 字符串列表 */
	readonly VarListStrEmpty: string[];
	/** 浮点数列表 */
	readonly VarListFloat: number[];
	/** 基础类型字典 */
	readonly VarDictIntFloat: Map<number, number>;
	/** 枚举类型字典 */
	readonly VarDictLongString: Map<number, string>;
	/** 类类型字典 */
	readonly VarDictStringClass: Map<string, SingleClass>;
	constructor(stream: any) {
		this.ID = stream.GetInt();
		this.Index = stream.GetInt();
		this.VarLong = stream.GetLong();
		this.VarFloat = stream.GetFloat();
		this.VarString = stream.GetString();
		this.VarListString = stream.GetList('String');
		this.VarListStrEmpty = stream.GetList('String');
		this.VarListFloat = stream.GetList('Float');
		this.VarDictIntFloat = stream.GetDict('Int', 'Float');
		this.VarDictLongString = stream.GetDict('Long', 'String');
		this.VarDictStringClass = stream.GetDict('String', 'AllTypeSingleClassMaker');
	}
}
Object.defineProperty(Stream.prototype, 'GetAllTypeCheckAll', {
	value: (stream: any) => new CheckAll(stream),
	writable: false,
});
/**  */
export class SingleClass {
	/** Var1 */
	readonly Var1: string;
	/** Var2 */
	readonly Var2: number;
	constructor(stream: any) {
		this.Var1 = stream.GetString();
		this.Var2 = stream.GetFloat();
	}
}
Object.defineProperty(Stream.prototype, 'GetAllTypeSingleClassMaker', {
	value: (stream: any) => stream[`Get${stream.GetString()}`].bind(stream),
	writable: false,
});
Object.defineProperty(Stream.prototype, 'GetAllTypeSingleClass', {
	value: (stream: any) => new SingleClass(stream),
	writable: false,
});
/**  */
export class M1 extends SingleClass {
	/** 继承1 */
	readonly V3: number;
	constructor(stream: any) {
		super(stream);
		this.V3 = stream.GetLong();
	}
}
Object.defineProperty(Stream.prototype, 'GetAllTypeM1', {
	value: (stream: any) => new M1(stream),
	writable: false,
});
/**  */
export class M2 extends SingleClass {
	/** 继承2 */
	readonly V4: boolean;
	constructor(stream: any) {
		super(stream);
		this.V4 = stream.GetBool();
	}
}
Object.defineProperty(Stream.prototype, 'GetAllTypeM2', {
	value: (stream: any) => new M2(stream),
	writable: false,
});

/**************************************** 声明与导出 *****************************************/
declare module '../CfgManager' {
	interface CfgManager {
		get AllTypeAllClassCfg(): AllClassCfg;
		get AllTypeCheckAllCfg(): CheckAllCfg;
	}
}

declare module '../Stream' {
	interface StreamBase {
		GetSingleClass(): SingleClass;
		GetM1(): M1;
		GetM2(): M2;
	}
}

