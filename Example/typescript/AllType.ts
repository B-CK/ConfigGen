import { Stream } from '../Stream';
/**************************************** 数据配置表 *****************************************/
/** AllClass表数据类*/
export class AllClassCfg extends Stream {
	static readonly refence = 'AllTypeAllClassCfg';
	get length() { return this._cfgs ? this._cfgs.length : 0; }
	get cfgs() { return this._cfgs; }
	private _cfgs: AllClass[] = [];
	private _key2idx: Map<number, number> = new Map();
	constructor(rootDir: string) {
		super(rootDir + 'alltype/allclass.da');
	}
	/**key索引数据(主键:ID)*/
	Get(key: number): AllClass | undefined {
		let idx = this._key2idx.get(key);
		if (idx == undefined) {
			console.error(`${this.path} key does not exist:${key}`);
			return undefined;
		}
		if (this.hasLoaded) {
			return this._cfgs[idx];
		}
		else {
			this.LoadConfig();
			return this._cfgs[idx];
		}
	}
	/**下标索引数据*/
	At(idx:number){
		if (this.hasLoaded) {
			return this._cfgs[idx];
		}
		else {
			this.LoadConfig();
			return this._cfgs[idx];
		}
	}
	protected ParseConfig() {
		this._cfgs = this.GetList('CfgAllTypeAllClass');
		for (let index = 0; index < this._cfgs.length; index++) {
			const item = this._cfgs[index];
			this._key2idx.set(item.ID, index);
		}
	}
	[Symbol.iterator]() { return this._cfgs.values(); }
}
/** CheckAll表数据类*/
export class CheckAllCfg extends Stream {
	static readonly refence = 'AllTypeCheckAllCfg';
	get length() { return this._cfgs ? this._cfgs.length : 0; }
	get cfgs() { return this._cfgs; }
	private _cfgs: CheckAll[] = [];
	private _key2idx: Map<number, number> = new Map();
	constructor(rootDir: string) {
		super(rootDir + 'alltype/checkall.da');
	}
	/**key索引数据(主键:ID)*/
	Get(key: number): CheckAll | undefined {
		let idx = this._key2idx.get(key);
		if (idx == undefined) {
			console.error(`${this.path} key does not exist:${key}`);
			return undefined;
		}
		if (this.hasLoaded) {
			return this._cfgs[idx];
		}
		else {
			this.LoadConfig();
			return this._cfgs[idx];
		}
	}
	/**下标索引数据*/
	At(idx:number){
		if (this.hasLoaded) {
			return this._cfgs[idx];
		}
		else {
			this.LoadConfig();
			return this._cfgs[idx];
		}
	}
	protected ParseConfig() {
		this._cfgs = this.GetList('CfgAllTypeCheckAll');
		for (let index = 0; index < this._cfgs.length; index++) {
			const item = this._cfgs[index];
			this._key2idx.set(item.ID, index);
		}
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
	constructor(o: any) {
		this.ID = o.GetInt();
		this.Index = o.GetInt();
		this.VarLong = o.GetLong();
		this.VarFloat = o.GetFloat();
		this.VarString = o.GetString();
		this.VarBool = o.GetBool();
		this.VarEnum = <CardElement>o.GetInt();
		this.VarClass = o.GetCfgAllTypeSingleClassMaker();
		this.VarListBase = o.GetList('String');
		this.VarListClass = o.GetList('CfgAllTypeSingleClassMaker');
		this.VarListCardElem = o.GetList('String');
		this.VarListFloat = o.GetList('Float');
		this.VarDictBase = o.GetDict('Int', 'Float');
		this.VarDictEnum = o.GetDict('Long', 'String');
		this.VarDictClass = o.GetDict('String', 'CfgAllTypeSingleClassMaker');
	}
}
Object.defineProperty(Stream.prototype, 'GetCfgAllTypeAllClass', {
	value: function(this: any) { return new AllClass(this); },
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
	constructor(o: any) {
		this.ID = o.GetInt();
		this.Index = o.GetInt();
		this.VarLong = o.GetLong();
		this.VarFloat = o.GetFloat();
		this.VarString = o.GetString();
		this.VarListString = o.GetList('String');
		this.VarListStrEmpty = o.GetList('String');
		this.VarListFloat = o.GetList('Float');
		this.VarDictIntFloat = o.GetDict('Int', 'Float');
		this.VarDictLongString = o.GetDict('Long', 'String');
		this.VarDictStringClass = o.GetDict('String', 'CfgAllTypeSingleClassMaker');
	}
}
Object.defineProperty(Stream.prototype, 'GetCfgAllTypeCheckAll', {
	value: function(this: any) { return new CheckAll(this); },
	writable: false,
});
/**  */
export class SingleClass {
	/** Var1 */
	readonly Var1: string;
	/** Var2 */
	readonly Var2: number;
	constructor(o: any) {
		this.Var1 = o.GetString();
		this.Var2 = o.GetFloat();
	}
}
Object.defineProperty(Stream.prototype, 'GetCfgAllTypeSingleClassMaker', {
	value: function (this: any) { return this[`Get${this.GetString().replace(/[\.]+/g, '')}`](); },
	writable: false,
});
Object.defineProperty(Stream.prototype, 'GetCfgAllTypeSingleClass', {
	value: function(this: any) { return new SingleClass(this); },
	writable: false,
});
/**  */
export class M1 extends SingleClass {
	/** 继承1 */
	readonly V3: number;
	constructor(o: any) {
		super(o);
		this.V3 = o.GetLong();
	}
}
Object.defineProperty(Stream.prototype, 'GetCfgAllTypeM1', {
	value: function(this: any) { return new M1(this); },
	writable: false,
});
/**  */
export class M2 extends SingleClass {
	/** 继承2 */
	readonly V4: boolean;
	constructor(o: any) {
		super(o);
		this.V4 = o.GetBool();
	}
}
Object.defineProperty(Stream.prototype, 'GetCfgAllTypeM2', {
	value: function(this: any) { return new M2(this); },
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

